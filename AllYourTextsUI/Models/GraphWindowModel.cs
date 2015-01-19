using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    public class GraphWindowModel : MainWindowModelBase, IGraphWindowModel
    {
        private GraphType _selectedGraphType;

        private IGraphDataGenerator _graphDataGenerator;

        private ITextGraphDataCollection _cachedCurrentGraphData;

        private AggregateConversation _aggregateConversation;

        private const int AggregateConversationIndex = 0;

        private const int DefaultConversationIndex = AggregateConversationIndex;

        private const int ManagerConversationsIndexOffset = AggregateConversationIndex + 1;

        private const GraphType DefaultGraphType = GraphType.AggregateHourOfDay;

        public GraphWindowModel(IDisplayOptions displayOptions, IPhoneSelectOptions phoneSelectOptions)
            :base(displayOptions, phoneSelectOptions)
        {
            SelectedConversationIndex = NoContactSelectedIndex;
            SelectedGraphType = DefaultGraphType;

            _graphDataGenerator = new GraphDataGenerator();

            _cachedCurrentGraphData = null;
        }

        private int SelectedConversationIndex
        {
            get
            {
                return _selectedConversationIndex;
            }
            set
            {
                _selectedConversationIndex = value;
                _cachedCurrentGraphData = null;
            }
        }

        public override IConversation SelectedConversation
        {
            get
            {
                if ((SelectedConversationIndex == -1) || (ConversationManager == null))
                {
                    return null;
                }
                else if (SelectedConversationIndex == AggregateConversationIndex)
                {
                    return _aggregateConversation;
                }
                else
                {
                    int managerIndex = ModelIndexToConversationManagerIndex(SelectedConversationIndex);

                    return ConversationManager.GetConversation(managerIndex);
                }
            }
            set
            {
                if (value == _aggregateConversation)
                {
                    SelectedConversationIndex = AggregateConversationIndex;
                }
                else
                {
                    int conversationIndex = ConversationManager.FindConversationIndex(value);
                    if (conversationIndex == -1)
                    {
                        SelectedConversationIndex = DefaultConversationIndex;
                    }
                    else
                    {
                        SelectedConversationIndex = ConversationManagerIndexToModelIndex(conversationIndex);
                    }
                }
            }
        }

        protected override IConversation DefaultConversation
        {
            get
            {
                return _aggregateConversation;
            }
        }

        public override IEnumerable<IConversationListItem> ConversationListItems
        {
            get
            {
                List<IConversationListItem> listItems = new List<IConversationListItem>();
                if (_aggregateConversation != null)
                {
                    listItems.Add(new ConversationListItem(_aggregateConversation));
                }
                listItems.AddRange(base.ConversationListItems);

                return listItems;
            }
        }

        public GraphType SelectedGraphType
        {
            get
            {
                return _selectedGraphType;
            }
            set
            {
                _selectedGraphType = value;
                _cachedCurrentGraphData = null;
            }
        }

        public ITextGraphDataCollection CurrentGraphDataCollection
        {
            get
            {
                if (_cachedCurrentGraphData == null)
                {
                    _cachedCurrentGraphData = GetGraphDataCollection();
                }

                return _cachedCurrentGraphData;
            }
        }

        public override IConversationManager ConversationManager
        {
            get
            {
                return base.ConversationManager;
            }
            set
            {
                if (value != null)
                {
                    base.ConversationManager = value;
                    _aggregateConversation = new AggregateConversation(value);
                    SelectedConversationIndex = DefaultConversationIndex;
                }
                else
                {
                    base.ConversationManager = null;
                    _aggregateConversation = null;
                    SelectedConversationIndex = NoContactSelectedIndex;
                }
                _cachedCurrentGraphData = null;
            }
        }

        private ITextGraphDataCollection GetGraphDataCollection()
        {
            if (SelectedConversation == null)
            {
                return null;
            }

            switch (SelectedGraphType)
            {
                case GraphType.AggregateHourOfDay:
                    return _graphDataGenerator.MessageCountAggregate(SelectedConversation, GraphAggregateType.HourOfDay);
                case GraphType.AggregateDayOfWeek:
                    return _graphDataGenerator.MessageCountAggregate(SelectedConversation, GraphAggregateType.DayOfWeek);
                case GraphType.PerMonth:
                    return _graphDataGenerator.MessageCountPerUnitTime(SelectedConversation, GraphTimeUnit.Month);
                default:
                    throw new ArgumentException();
            }

            throw new ArgumentException();
        }

        private int ModelIndexToConversationManagerIndex(int modelIndex)
        {
            return modelIndex - ManagerConversationsIndexOffset;
        }

        private int ConversationManagerIndexToModelIndex(int conversationManagerIndex)
        {
            return conversationManagerIndex + ManagerConversationsIndexOffset;
        }
    }
}
