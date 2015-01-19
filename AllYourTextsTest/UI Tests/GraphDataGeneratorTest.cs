using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsUi.Framework;
using DummyData;
using System.Collections.Generic;
using AllYourTextsLib.Framework;
using AllYourTextsLib;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for GraphDataGeneratorTest and is intended
    ///to contain all GraphDataGeneratorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GraphDataGeneratorTest
    {

        public List<ITextGraphData> GetConversationGraphData(DummyPhoneNumberId DummyPhoneNumberId, GraphTimeUnit timeUnit)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId);
            GraphDataGenerator generator = new GraphDataGenerator();
            return new List<ITextGraphData>(generator.MessageCountPerUnitTime(conversation, timeUnit));
        }

        public List<ITextGraphData> GetAggregateConversationGraphData(DummyPhoneNumberId DummyPhoneNumberId, GraphAggregateType aggregateType)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId);
            GraphDataGenerator generator = new GraphDataGenerator();
            return new List<ITextGraphData>(generator.MessageCountAggregate(conversation, aggregateType));
        }

        public void VerifyGraphDataCollectionsEqual(List<ITextGraphData> graphDataCollectionExpected, List<ITextGraphData> graphDataCollectionActual)
        {
            Assert.AreEqual(graphDataCollectionExpected.Count, graphDataCollectionActual.Count);

            //
            // Order is not guaranteed to match, so just check that all expected items appear in actual results.
            //

            foreach (ITextGraphData graphDataExpected in graphDataCollectionExpected)
            {
                Assert.IsTrue(graphDataCollectionActual.Contains(graphDataExpected));
            }
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void EmptyPerUnitTimeTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.NeverTexterCell);
            GraphDataGenerator generator = new GraphDataGenerator();

            ITextGraphDataCollection perMonthCollection = generator.MessageCountPerUnitTime(conversation, GraphTimeUnit.Month);

            Assert.AreEqual(0, perMonthCollection.Count);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void EmptyAggregateTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.NeverTexterCell);
            GraphDataGenerator generator = new GraphDataGenerator();

            ITextGraphDataCollection hourOfDayCollection = generator.MessageCountAggregate(conversation, GraphAggregateType.HourOfDay);
            foreach (ITextGraphData graphData in hourOfDayCollection)
            {
                Assert.AreEqual(0, graphData.MessagesTotal);
            }

            ITextGraphDataCollection dayOfWeekCollection = generator.MessageCountAggregate(conversation, GraphAggregateType.DayOfWeek);
            foreach (ITextGraphData graphData in dayOfWeekCollection)
            {
                Assert.AreEqual(0, graphData.MessagesTotal);
            }
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidArgAggregateTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.TonyWolfCell);
            GraphDataGenerator generator = new GraphDataGenerator();

            generator.MessageCountAggregate(conversation, GraphAggregateType.Unknown);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidArgPerUnitTimeTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.TonyWolfCell);
            GraphDataGenerator generator = new GraphDataGenerator();
            
            generator.MessageCountPerUnitTime(conversation, GraphTimeUnit.Unknown);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetPerMonthDataCollectionTest()
        {
            List<ITextGraphData> graphDataActual = GetConversationGraphData(DummyPhoneNumberId.TonyWolfCell, GraphTimeUnit.Month);

            List<ITextGraphData> graphDataExpected = new List<ITextGraphData>();
            graphDataExpected.Add(new TextGraphData(new DateTime(2008, 12, 1), 4 + 4));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 1, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 2, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 3, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 4, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 5, 1), 5));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 6, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 7, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 8, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 9, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 10, 1), 5 + 3));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 11, 1), 12));
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 12, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 1, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 2, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 3, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 4, 1), 8));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 5, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 6, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 7, 1), 0));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 8, 1), 10));

            VerifyGraphDataCollectionsEqual(graphDataExpected, graphDataActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetPerMonthDataNonSequentialTest()
        {
            MockConversation conversation = new MockConversation();
            conversation.AddMessage(new TextMessage(109, false, new DateTime(2010, 1, 15), "whatever1", "12125551424", CountryCallingCodeFinder.CountryAbbreviationUnitedStates));
            conversation.AddMessage(new TextMessage(110, false, new DateTime(2010, 2, 12), "whatever2", "12125551424", CountryCallingCodeFinder.CountryAbbreviationUnitedStates));
            conversation.AddMessage(new TextMessage(111, false, new DateTime(2010, 1, 8), "whatever3", "12125551424", CountryCallingCodeFinder.CountryAbbreviationUnitedStates));
            conversation.AddMessage(new TextMessage(112, false, new DateTime(2009, 12, 3), "whatever3", "12125551424", CountryCallingCodeFinder.CountryAbbreviationUnitedStates));

            GraphDataGenerator generator = new GraphDataGenerator();

            List<ITextGraphData> graphDataActual = new List<ITextGraphData>(generator.MessageCountPerUnitTime(conversation, GraphTimeUnit.Month));

            List<ITextGraphData> graphDataExpected = new List<ITextGraphData>();
            graphDataExpected.Add(new TextGraphData(new DateTime(2009, 12, 1), 1));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 1, 1), 2));
            graphDataExpected.Add(new TextGraphData(new DateTime(2010, 2, 1), 1));

            VerifyGraphDataCollectionsEqual(graphDataExpected, graphDataActual);
        }

        private List<ITextGraphData> PerHourArrayToGraphDataList(int[] perHourArray)
        {
            List<ITextGraphData> graphData = new List<ITextGraphData>(perHourArray.Length);

            for (int i = 0; i < perHourArray.Length; i++)
            {
                graphData.Add(new TextGraphData(GraphDataGenerator.NormalizedSunday.AddHours(i), perHourArray[i]));
            }

            return graphData;
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetAggregateHourOfDayDataCollectionTest()
        {
            List<ITextGraphData> graphDataActual = GetAggregateConversationGraphData(DummyPhoneNumberId.TonyWolfCell, GraphAggregateType.HourOfDay);

            int[] graphDataExpectedArray = new int[24];
            graphDataExpectedArray[9] = 5;
            graphDataExpectedArray[10] = 8 + 6;
            graphDataExpectedArray[11] = 4;
            graphDataExpectedArray[13] = 4 + 5;
            graphDataExpectedArray[14] = 1 + 9;
            graphDataExpectedArray[15] = 3 + 3;
            graphDataExpectedArray[17] = 3;

            List<ITextGraphData> graphDataExpected = PerHourArrayToGraphDataList(graphDataExpectedArray);
            
            VerifyGraphDataCollectionsEqual(graphDataExpected, graphDataActual);
        }
        
        private List<ITextGraphData> PerDayOfWeekArrayToGraphDataList(int[] perDayOfWeekArray)
        {
            List<ITextGraphData> graphData = new List<ITextGraphData>(perDayOfWeekArray.Length);

            for (int i = 0; i < perDayOfWeekArray.Length; i++)
            {
                graphData.Add(new TextGraphData(GraphDataGenerator.NormalizedSunday.AddDays(i), perDayOfWeekArray[i]));
            }

            return graphData;
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetAggregateDayOfWeekDataCollectionTest()
        {
            List<ITextGraphData> graphDataActual = GetAggregateConversationGraphData(DummyPhoneNumberId.TonyWolfCell, GraphAggregateType.DayOfWeek);

            int[] graphDataExpectedArray = new int[7];
            graphDataExpectedArray[0] = 5;          // Sunday
            graphDataExpectedArray[1] = 4 + 12;     // Monday
            graphDataExpectedArray[2] = 8;          // Tuesday
            graphDataExpectedArray[3] = 0;          // Wednesday
            graphDataExpectedArray[4] = 4 + 10;     // Thursday
            graphDataExpectedArray[5] = 8;          // Friday
            graphDataExpectedArray[6] = 0;          // Saturday

            List<ITextGraphData> graphDataExpected = PerDayOfWeekArrayToGraphDataList(graphDataExpectedArray);

            VerifyGraphDataCollectionsEqual(graphDataExpected, graphDataActual);
        }
    }
}
