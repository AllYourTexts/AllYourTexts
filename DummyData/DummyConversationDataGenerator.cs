using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using Moq;

namespace DummyData
{

    public enum DummyPhoneNumberId
    {
        AnthonyWeinerCell = 0,
        BobbyCssOffice,
        CrackerJackOffice,
        DavolaCell,
        DavolaiPhone,
        DavolaDuplicateCell,
        FrankieCoolPicsCell,
        ObamaCell,
        HarryLooseTieCell,
        IsraeliDanCell,
        JerryCoolVidsCell,
        LongTalkerCell,
        NeverTexterHome,
        NeverTexterCell,
        RandMcNallyCell,
        ReliableLarryOffice,
        StacyStevensCell,
        TonyWolfCell,
        VictoriaWolfCell,
        WolfmanJackCell,
        UnknownLawnmower,
        UnknownEagle,
        UnknownGrahamBell
    }

    public enum DummyContactId
    {
        AnthonyWeiner = 0,
        BobbyCss,
        CrackerJack,
        Davola,
        DavolaNumberDuplicate,
        FrankieCoolPics,
        HarryLooseTie,
        IsraeliDan,
        JerryCoolVids,
        LongTalker,
        Obama,
        NeverTexter,
        RandMcNally,
        ReliableLarry,
        StacyStevens,
        TonyWolf,
        VictoriaWolf,
        WolfmanJack,
    }

    public enum DummyChatRoomId
    {
        ChatRoomA = 0,
        ChatRoomB,
        ChatRoomC,
        ChatRoomD       // 1 unknown contact, 1 known
    }

    public class DummyConversationDataGenerator
    {

        private static string[] _DummyPhoneNumbers;
        private static string[] _DummyPhoneNumbersStripped;
        private static Contact[] _DummyContacts;
        private static Dictionary<string, MessageSet> _DummyMessageSets;
        private static ChatRoomInformation[] _DummyChatRoomInfoItems;

        static DummyConversationDataGenerator()
        {
            InitPhoneNumbers();
            InitChatRoomInfoItems();
            InitMessageSets();
        }

        private static void InitPhoneNumberFormatted(DummyPhoneNumberId phoneNumberId, string phoneNumberFormatted)
        {
            InitPhoneNumberDual(phoneNumberId, phoneNumberFormatted, null);
        }

        private static void InitPhoneNumberStripped(DummyPhoneNumberId phoneNumberId, string phoneNumberStripped)
        {
            InitPhoneNumberDual(phoneNumberId, phoneNumberStripped, phoneNumberStripped);
        }

        private static void InitPhoneNumberDual(DummyPhoneNumberId phoneNumberId, string phoneNumber, string phoneNumberStripped)
        {
            int phoneNumberIdInt = (int)phoneNumberId;
            _DummyPhoneNumbers[phoneNumberIdInt] = phoneNumber;
            _DummyPhoneNumbersStripped[phoneNumberIdInt] = phoneNumberStripped;
        }

        private static void InitPhoneNumber(DummyContactId contactId, DummyPhoneNumberId phoneNumberId, string firstName, string lastName, string phoneNumberValue, string phoneNumberValueStripped)
        {
            IPhoneNumber phoneNumber = new PhoneNumber(phoneNumberValue);
            Contact contact = new Contact((long)contactId, firstName, null, lastName, phoneNumber);

            InitPhoneNumberDual(phoneNumberId, phoneNumberValue, phoneNumberValueStripped);

            _DummyContacts[(int)phoneNumberId] = contact;
        }

        private static void InitPhoneNumberUnknown(DummyPhoneNumberId phoneNumberId, string phoneNumber)
        {
            Contact contactUnknown = new Contact(Contact.UnknownContactId, null, null, null, new PhoneNumber(phoneNumber));

            InitPhoneNumberDual(phoneNumberId, phoneNumber, PhoneNumber.Strip(new PhoneNumber(phoneNumber)));

            _DummyContacts[(int)phoneNumberId] = contactUnknown;
        }

        private static void InitPhoneNumbers()
        {
            int phoneNumberIdCount = Enum.GetValues(typeof(DummyPhoneNumberId)).Length;
            _DummyPhoneNumbers = new string[phoneNumberIdCount];
            _DummyPhoneNumbersStripped = new string[phoneNumberIdCount];
            _DummyContacts = new Contact[phoneNumberIdCount];

            InitPhoneNumber(DummyContactId.AnthonyWeiner, DummyPhoneNumberId.AnthonyWeinerCell, "Anthony", "Weiner", "1 (212) 555-8868", "2125558868");
            InitPhoneNumber(DummyContactId.BobbyCss, DummyPhoneNumberId.BobbyCssOffice, "Bo<b>b</b>by", "Css", "1 (206) 555-1974", "2065551974");
            InitPhoneNumber(DummyContactId.CrackerJack, DummyPhoneNumberId.CrackerJackOffice, "Cracker\\Jack", "", "*9977", "*9977");
            InitPhoneNumber(DummyContactId.Davola, DummyPhoneNumberId.DavolaCell, "Joe", "Davola", "1 (212) 555-8728", "2125558728");
            InitPhoneNumber(DummyContactId.Davola, DummyPhoneNumberId.DavolaiPhone, "Joe", "Davola", "646-555-9189", "6465559189");
            InitPhoneNumber(DummyContactId.DavolaNumberDuplicate, DummyPhoneNumberId.DavolaDuplicateCell, "Joe", "D", "1 (212) 555-8728", "2125558728");
            InitPhoneNumber(DummyContactId.FrankieCoolPics, DummyPhoneNumberId.FrankieCoolPicsCell, "Frankie", "Coolpics", "1 (505) 555-8823", "5055558823");
            InitPhoneNumber(DummyContactId.Obama, DummyPhoneNumberId.ObamaCell, "Barack", "Obama", "1 (202) 555-1600", "2025551600");
            InitPhoneNumber(DummyContactId.HarryLooseTie, DummyPhoneNumberId.HarryLooseTieCell, "Harry", "Loosetie", "1 (308) 555-2389", "3085552389");
            InitPhoneNumber(DummyContactId.IsraeliDan, DummyPhoneNumberId.IsraeliDanCell, "מיכה", "פול", "+972 052 311 5679", "523115679");
            InitPhoneNumber(DummyContactId.JerryCoolVids, DummyPhoneNumberId.JerryCoolVidsCell, "Jerry", "Coolvids", "+1-919-555-3656", "9195553656");
            InitPhoneNumber(DummyContactId.LongTalker, DummyPhoneNumberId.LongTalkerCell, "Lionel", "Longtalker", "+1-916-555-1977", "9165551977");
            InitPhoneNumber(DummyContactId.NeverTexter, DummyPhoneNumberId.NeverTexterHome, "Wally", "Nevertexter", "+1 (914) 555-1129", "9145551129");
            InitPhoneNumber(DummyContactId.NeverTexter, DummyPhoneNumberId.NeverTexterCell, "Wally", "Nevertexter", "+1 (914) 555-8294", "9145558294");
            InitPhoneNumber(DummyContactId.RandMcNally, DummyPhoneNumberId.RandMcNallyCell, "Rand", "McNally", "212-555-0992", "2125550992");
            InitPhoneNumber(DummyContactId.ReliableLarry, DummyPhoneNumberId.ReliableLarryOffice, "Reliable Larry's", "Wake-Up Service", "709-555-3473", "7095553473");
            InitPhoneNumber(DummyContactId.StacyStevens, DummyPhoneNumberId.StacyStevensCell, "Stacy", "Stevens", "503-555-1268", "5035551268");
            InitPhoneNumber(DummyContactId.TonyWolf, DummyPhoneNumberId.TonyWolfCell, "Tony", "Harver", "1 (305) 555-0925", "3055550925");
            InitPhoneNumber(DummyContactId.VictoriaWolf, DummyPhoneNumberId.VictoriaWolfCell, "Victoria", "Harver", "1 (305) 555-7260", "3055557260");
            InitPhoneNumber(DummyContactId.WolfmanJack, DummyPhoneNumberId.WolfmanJackCell, "Wolfman", "Jack", "(202) 555-6253", "2025556253");
            InitPhoneNumberUnknown(DummyPhoneNumberId.UnknownLawnmower, "2125559028");
            InitPhoneNumberUnknown(DummyPhoneNumberId.UnknownEagle, "8275550972");
            InitPhoneNumberUnknown(DummyPhoneNumberId.UnknownGrahamBell, "1");
        }

        private static void InitChatRoomInfoItems()
        {
            int chatRoomInfoItemCount = Enum.GetValues(typeof(DummyChatRoomId)).Length;
            ChatRoomInformation[] dummyChatRoomInfoItems = new ChatRoomInformation[chatRoomInfoItemCount];

            {
                DummyChatRoomId chatId = DummyChatRoomId.ChatRoomA;
                string[] participants = {
                                            GetPhoneNumber(DummyPhoneNumberId.ObamaCell),
                                            GetPhoneNumber(DummyPhoneNumberId.AnthonyWeinerCell)
                                        };

                dummyChatRoomInfoItems[(int)chatId] = new ChatRoomInformation("chat901258305184729544", participants);
            }

            {
                DummyChatRoomId chatId = DummyChatRoomId.ChatRoomB;
                string[] participants = {
                                            GetPhoneNumber(DummyPhoneNumberId.TonyWolfCell),
                                            GetPhoneNumber(DummyPhoneNumberId.VictoriaWolfCell)
                                        };

                dummyChatRoomInfoItems[(int)chatId] = new ChatRoomInformation("chat901258305184729566", participants);
            }

            {
                DummyChatRoomId chatId = DummyChatRoomId.ChatRoomC;
                string[] participants = {
                                            GetPhoneNumber(DummyPhoneNumberId.ObamaCell),
                                            GetPhoneNumber(DummyPhoneNumberId.HarryLooseTieCell),
                                            GetPhoneNumber(DummyPhoneNumberId.AnthonyWeinerCell),
                                            GetPhoneNumber(DummyPhoneNumberId.ReliableLarryOffice)
                                        };

                dummyChatRoomInfoItems[(int)chatId] = new ChatRoomInformation("chat901258305184725792", participants);
            }

            {
                DummyChatRoomId chatId = DummyChatRoomId.ChatRoomD;
                string[] participants = {
                                            GetPhoneNumber(DummyPhoneNumberId.HarryLooseTieCell),
                                            GetPhoneNumber(DummyPhoneNumberId.UnknownLawnmower)
                                        };

                dummyChatRoomInfoItems[(int)chatId] = new ChatRoomInformation("chat901258305184724753", participants);
            }

            foreach (ChatRoomInformation cri in dummyChatRoomInfoItems)
            {
                if (cri == null)
                {
                    throw new ArgumentException("You forgot to initialize a chat room information item.");
                }
            }

            _DummyChatRoomInfoItems = dummyChatRoomInfoItems;
        }

        private static void InitMessageSet(Dictionary<string, MessageSet> messageSets, string messageSetKey, MockMessageGeneratorBase messageGenerator)
        {
            messageSets[messageSetKey] = new MessageSet(messageGenerator.GetMessages());
        }

        private static void InitMessageSets()
        {
            Dictionary<string, MessageSet> dummyMessages = new Dictionary<string, MessageSet>();
            string phoneNumberStripped;
            string chatRoomName;
            MockMessageGenerator messageGenerator;
            MockChatMessageGenerator chatMessageGenerator;

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.AnthonyWeinerCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 3, 18, 16, 32, 14);
            messageGenerator.AddOutgoingMessage(0.0, "Hi Congressman Weiner. I'm really glad you stood up for NPR in yesterday's session. I hope you can prevent their funding from being cut!");
            messageGenerator.AddIncomingMessage(38.0, "I'll do everything I can!");
            messageGenerator.AddIncomingMessage(8003.0, "Are you a young, attractive college co-ed by chance?");
            messageGenerator.AddOutgoingMessage(48.0, "What?");
            messageGenerator.AddIncomingMessage(47.0, "nvm, just looked you up on facebook. Let's keep in touch!");
            messageGenerator.SetCurrentTime(2011, 5, 29, 23, 08, 34);
            messageGenerator.AddIncomingMessage(0.0, "Hey check your twitter. I tweeted you a really funny photo!");
            messageGenerator.AddOutgoingMessage(45.0, "Um... I'm not sure I want to.");
            messageGenerator.AddIncomingMessage(38.0, "No, check it. I think you'll enjoy it. It's a play on my name, if you know what I mean...");
            messageGenerator.AddOutgoingMessage(203.0, "Aren't you married?");
            messageGenerator.AddIncomingMessage(34.0, "Don't worry about that. Just look at the picture.");
            messageGenerator.AddOutgoingMessage(508.0, "This is just a picture of a dachsund.");
            messageGenerator.AddOutgoingMessage(28.0, "Oh. I get it.");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.BobbyCssOffice);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 10, 23, 21, 45, 06);
            messageGenerator.AddOutgoingMessage(0.0, "How do you make stuff bold, Bobby?");
            messageGenerator.AddIncomingMessage(18.0, "Use the <b> tag!");
            messageGenerator.AddOutgoingMessage(20.0, "And how do I do a right single quote?");
            messageGenerator.AddIncomingMessage(19.0, "It's as easy as &rsquo;!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.CrackerJackOffice);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 10, 17, 15, 45, 40);
            messageGenerator.AddOutgoingMessage(0.0, "Whatup Cracker\\Jack? Love the new number.");
            messageGenerator.AddIncomingMessage(3.0, "Thanks, dog!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.DavolaCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2008, 2, 24, 10, 36, 54);
            messageGenerator.AddIncomingMessage(0.0, "Hi!");
            messageGenerator.AddIncomingMessage(35.0, "How's it going?");
            messageGenerator.AddIncomingMessage(32.0, "You going to the party later?");
            messageGenerator.AddIncomingMessage(138.0, "Answer me!");
            messageGenerator.AddOutgoingMessage(205.0, "Hey, sorry was in the shower. Yeah I'll probably check it out.");
            messageGenerator.AddIncomingMessage(29.0, "Good. Gonna be off the hizzy!");
            messageGenerator.AddOutgoingMessage(25.0, "Cool, is tracy coming?");
            messageGenerator.AddIncomingMessage(400.0, "Nah, she's got a restraining order against me.");
            messageGenerator.AddOutgoingMessage(125.0, "Oh! Bummerpants!");
            messageGenerator.AddIncomingMessage(25.0, "Yeah, seriously.");
            messageGenerator.AddOutgoingMessage(225.0, "Against me too. High five!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.DavolaiPhone);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2008, 2, 28, 10, 36, 54);
            messageGenerator.AddIncomingMessage(0.0, "Dude! I just got an iPhone!");
            messageGenerator.AddOutgoingMessage(35.0, "Yeah? Is it god?");
            messageGenerator.AddIncomingMessage(3.0, "Yes");
            messageGenerator.AddOutgoingMessage(5.0, "good*");
            messageGenerator.AddIncomingMessage(18.0, "Oh.");
            messageGenerator.AddIncomingMessage(3.0, "Awk-ward....");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.FrankieCoolPicsCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2012, 9, 9, 20, 34, 15);
            IMessageAttachment attachment = new MessageAttachment(0, AttachmentType.Image, @"C:\fakepath\backup\082308302382", "IMG_0036.JPG");
            messageGenerator.AddIncomingMessageWithAttachment(0, "Check this shit out!", attachment);
            messageGenerator.AddOutgoingMessage(15.0, "Crazy!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.LongTalkerCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2010, 9, 9, 20, 34, 15);
            for (int i = 0; i < 100000; i++)
            {
                messageGenerator.AddOutgoingMessage(15.0, "what up?");
                messageGenerator.AddIncomingMessage(2.0, "nm, u?");
                messageGenerator.AddOutgoingMessage(2.0, "nm");
            }
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.ObamaCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2008, 11, 4, 22, 18, 5);
            messageGenerator.AddOutgoingMessage(0.0, "Congrats, buddy!");
            messageGenerator.AddIncomingMessage(35.0, "Thanks. Couldn't have done it without you!");
            messageGenerator.AddOutgoingMessage(438.0, "np");
            messageGenerator.SetCurrentTime(2011, 5, 1, 8, 45, 09);
            messageGenerator.AddOutgoingMessage(138.0, "Yo, I think I know where Osama Bin Laden is hiding?");
            messageGenerator.AddIncomingMessage(205.0, "o rly?");
            messageGenerator.AddOutgoingMessage(29.0, "Yeah, dude. Abottabad, Pakistan. Huge compound. Can't miss it.");
            messageGenerator.AddIncomingMessage(25.0, "Sweet. I'll send some navy seals.");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.HarryLooseTieCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2010, 5, 7, 13, 14, 32);
            messageGenerator.AddOutgoingMessage(0.0, "Happy birthday!");
            messageGenerator.AddIncomingMessage(305.0, "Thanks!.");
            messageGenerator.SetCurrentTime(2011, 5, 7, 15, 17, 18);
            messageGenerator.AddOutgoingMessage(0.0, "Happy b-bday, buddy!");
            messageGenerator.AddIncomingMessage(208.0, "ty");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.IsraeliDanCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2013, 8, 1, 21, 38, 12);
            messageGenerator.AddOutgoingMessage(0.0, "האם את יוצאת מחר ובאיזו שעה?");
            messageGenerator.AddIncomingMessage(205.0, "כן, בסביבות 6 ועשרים");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.JerryCoolVidsCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2012, 9, 9, 20, 34, 15);
            IMessageAttachment videoAttachment = new MessageAttachment(0, AttachmentType.Video, @"C:\fakepath\backup\056798632135464", "VIDEO_0015.MOV");
            messageGenerator.AddIncomingMessageWithAttachment(0, "It's a video of me doing a backflip!", videoAttachment);
            messageGenerator.AddOutgoingMessage(126.0, "Badass!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.ReliableLarryOffice);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2013, 6, 19, 17, 34, 18);
            messageGenerator.AddOutgoingMessage(0.0, "Randyboy! How was your weekend?");
            messageGenerator.AddIncomingMessage(38.0, "Cartographic. Yourself?");
            messageGenerator.AddOutgoingMessage(28.0, "Amazing! Amazing. I took a trip to Tupper Lake. Do you know where that is?");
            messageGenerator.AddIncomingMessage(16.0, "Yes, I do.");
            messageGenerator.SetCurrentTime(2013, 7, 3, 8, 04, 45);
            messageGenerator.AddOutgoingMessage(0.0, "Hey, dude. I hate to ask but can you give me Tony Harver's home address? I forgot it.");
            messageGenerator.AddIncomingMessage(35.0, "It's 1388 Wilxbury Road. This is why I gave you that map.");
            messageGenerator.AddOutgoingMessage(15.0, "I know. I know. Thanks for the address. You're a lifesaver, buddy!");
            messageGenerator.SetCurrentTime(2013, 8, 15, 15, 24, 29);
            messageGenerator.AddOutgoingMessage(0.0, "Man, have you tried Google Earth? It's so cool!");
            messageGenerator.AddIncomingMessage(24.0, "No it's not. It doesn't compare to a globe that you can hold in your hand.");
            messageGenerator.AddOutgoingMessage(12.0, "Whoa! I can see my house. Can a physical globe do that?");
            messageGenerator.AddIncomingMessage(15.0, "No. No it cannot.");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.ReliableLarryOffice);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 6, 13, 7, 0, 0);
            messageGenerator.AddIncomingMessage(0.0, "Good morning! This is your 7 AM wake up text. Thanks for choosing Reliable Larry's Wake-Up Service and have a great day!");
            messageGenerator.AddOutgoingMessage(28.0, "Thanks!");
            messageGenerator.SetCurrentTime(2011, 6, 14, 7, 0, 0);
            messageGenerator.AddIncomingMessage(0.0, "Good morning! This is your 7 AM wake up text. Weather in Framingdale is sunny and 75. Have a great day!");
            messageGenerator.AddOutgoingMessage(42.0, "Thanks, Reliable Larry!");
            messageGenerator.SetCurrentTime(2011, 6, 15, 8, 9, 38);
            messageGenerator.AddIncomingMessage(0.0, "Good morning! This is your 7 AM wake up text. Have a great day!");
            messageGenerator.AddOutgoingMessage(11.0, "It's 8:09...");
            messageGenerator.AddIncomingMessage(48.0, "Yeah, sorry about that. My wake up guy forgot to text me this morning.");
            messageGenerator.SetCurrentTime(2011, 6, 16, 4, 0, 0);
            messageGenerator.AddIncomingMessage(0.0, "Good morning! This is your 7 AM wake up text. Weather in Miami is gorgeous today!");
            messageGenerator.AddOutgoingMessage(96.0, "Wtf? It's 4 AM! You weren't supposed to text me for another 3 hours.");
            messageGenerator.AddIncomingMessage(42.0, "As I said, I'm in Miami... It's 7 AM here. Wake up sleepyhead!");
            messageGenerator.AddOutgoingMessage(106.0, "I want to wake up at 7 AM in *my* time zone.");
            messageGenerator.AddIncomingMessage(82.0, "Oh, we can certainly accomodate that, sir. I've made a note in my records. Thank you for choosing Reliable Larry!");
            messageGenerator.SetCurrentTime(2011, 6, 18, 14, 57, 09);
            messageGenerator.AddIncomingMessage(0.0, "Good morning! This is your 7 AM wake up text (for yesterday). Sorry for the delay. I was reading a really cool magazine.");
            messageGenerator.AddIncomingMessage(8922.0, "This is a confirmation that your request to close your account with Reliable Larry's Wake-Up Service has been processed. Thank you for your business and please choose Reliable Larry for all of your future wake-up related needs!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.StacyStevensCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2013, 9, 16, 18, 15, 06);
            messageGenerator.AddOutgoingMessage(0.0, "Stacy, have you tried AllYourTexts? It's great!");
            messageGenerator.AddIncomingMessage(28.0, "We're broken up. Stop texting me.");
            messageGenerator.AddOutgoingMessage(32.0, "You can read all your iPhone texts from your PC! This is awesome!");
            messageGenerator.AddIncomingMessage(104.0, "I don't care");
            messageGenerator.SetCurrentTime(2013, 9, 17, 9, 29, 29);
            messageGenerator.AddOutgoingMessage(0.0, "It's only $4.99. What an amazing deal.");
            messageGenerator.AddIncomingMessage(42.0, "That doesn't make up for you forgetting my birthday. And our anniversary. They're on the same day!");
            messageGenerator.AddOutgoingMessage(26.0, "Yeah, but there's a 15-day free trial. Take that into consideration.");
            messageGenerator.AddIncomingMessage(38.0, "No.");
            messageGenerator.SetCurrentTime(2013, 9, 18, 14, 02, 15);
            messageGenerator.AddOutgoingMessage(0.0, "Whoa! It also graphs all your texting behavior. You can see your texting behavior hourly or per day of week.");
            messageGenerator.AddIncomingMessage(42.0, "Are you kidding me?");
            messageGenerator.AddIncomingMessage(42.0, "Because that *does* sound awesome!");
            messageGenerator.AddOutgoingMessage(26.0, "That's what I've been trying to tell you!!!");
            messageGenerator.AddIncomingMessage(38.0, "Let's get back together. I still love you and your taste in useful applications!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.TonyWolfCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2008, 12, 22, 14, 27, 15);
            messageGenerator.AddOutgoingMessage(0.0, "Hey, how was your morning?");
            messageGenerator.AddIncomingMessage(3898.0, "Pretty good, I guess. why?");
            messageGenerator.AddOutgoingMessage(29.0, "Bet you flipped out when you realized the keyboard in your office was 3 inches left of its usual placement. that was me! OWNED!");
            messageGenerator.AddIncomingMessage(709.0, "Oh... you got me good I guess.");
            messageGenerator.SetCurrentTime(2008, 12, 25, 13, 48, 06);
            messageGenerator.AddOutgoingMessage(0.0, "Hey tony. Merry Christmas! May Jesus bless you and your family enjoy this special day.");
            messageGenerator.AddIncomingMessage(208.0, "Thanks! Happy holidays to you as well!");
            messageGenerator.AddOutgoingMessage(42.0, "HA! Got you! I'm Jewish! I don't even celebrate Christmas or believe in Jesus and thus my prayer was invalid and null. OWNED!");
            messageGenerator.AddIncomingMessage(89.0, "Um... okay.");
            messageGenerator.SetCurrentTime(2009, 5, 3, 9, 27, 32);
            messageGenerator.AddOutgoingMessage(0.0, "Hey tony, you know if it's supposed to rain today?");
            messageGenerator.AddIncomingMessage(145.0, "No, not sure.");
            messageGenerator.AddOutgoingMessage(83.0, "What's the matter? Didn't check the forecast in the newspaper. Oh, that's right. I took your newspaper this morning. HAHA! OWNED!");
            messageGenerator.AddIncomingMessage(89.0, "Look, that's more a matter of theft than pranking. Please don't do that again.");
            messageGenerator.AddOutgoingMessage(200.0, "Duh, what kind of a prankster mastermind does the same prank twice?");
            messageGenerator.SetCurrentTime(2009, 10, 6, 13, 25, 17);
            messageGenerator.AddOutgoingMessage(0.0, "Hey, is the Internet down for you, too?");
            messageGenerator.AddIncomingMessage(145.0, "Yeah, what's going on? I need to get the order out by 5 or we're screwed.");
            messageGenerator.AddOutgoingMessage(839.0, "I sure hope it comes back up :)");
            messageGenerator.AddIncomingMessage(50.0, "Why are you writing a smiley. Is this one of your pranks?");
            messageGenerator.AddOutgoingMessage(89.0, "No, of course not.");
            messageGenerator.SetCurrentTime(2009, 10, 6, 17, 15, 04);
            messageGenerator.AddOutgoingMessage(0.0, "Haha! I got you! I disconnected your Ethernet cable. HAHA! OWNED!");
            messageGenerator.AddIncomingMessage(22.0, "You idiot! We're going to lose thousands of dollars on this account.");
            messageGenerator.AddOutgoingMessage(38.0, "A small price to pay. Owned so hard lol!");
            messageGenerator.SetCurrentTime(2009, 11, 9, 14, 25, 44);
            messageGenerator.AddOutgoingMessage(0.0, "Tony, I have a confession to make.");
            messageGenerator.AddIncomingMessage(87.0, "Are you finally going to take responsibility for losing the Sherman account?");
            messageGenerator.AddOutgoingMessage(32.0, "No, this is more important than that.");
            messageGenerator.AddOutgoingMessage(19.0, "Also, that was more your fault for not checking your Ethernet cable.");
            messageGenerator.AddIncomingMessage(304.0, "What is it then?");
            messageGenerator.AddOutgoingMessage(76.0, "I've been sleeping with your wife!");
            messageGenerator.AddIncomingMessage(234.0, "Yeah right");
            messageGenerator.AddOutgoingMessage(29.0, "You can ask her. We've been all affairing and stuff.");
            messageGenerator.AddIncomingMessage(28.0, "Fine I will.");
            messageGenerator.AddIncomingMessage(2200.0, "You bastard! Where are you? I want to see you face to face.");
            messageGenerator.AddOutgoingMessage(98.0, "Haha, gotcha! Your wife was in on it! She would never actually sleep with me; she's an anti-semite. HAHA OWNED!");
            messageGenerator.AddIncomingMessage(105.0, "I hate you.");
            messageGenerator.SetCurrentTime(2010, 4, 2, 10, 52, 04);
            messageGenerator.AddOutgoingMessage(0.0, "Tony! There's been a terrible accident! Your kids are at the hospital in critical condition!");
            messageGenerator.AddIncomingMessage(22.0, "Oh my god! What happened?");
            messageGenerator.AddOutgoingMessage(38.0, "Car accident. Really terrible.");
            messageGenerator.AddIncomingMessage(22.0, "Are you serious? Is this another one of your pranks?");
            messageGenerator.AddIncomingMessage(42.0, "This is important! Tell me!");
            messageGenerator.AddOutgoingMessage(27.0, "Haha! Yes! Got you! OWNED!");
            messageGenerator.AddIncomingMessage(45.0, "Oh, thank god. So my kids are okay?");
            messageGenerator.AddOutgoingMessage(38.0, "No, I told you. They're in the hospital. The prank was that I was the one who ran them over. OWNED!");
            messageGenerator.SetCurrentTime(2010, 8, 19, 10, 52, 04);
            messageGenerator.AddOutgoingMessage(0.0, "Tony! There's a wolf loose in the building! It's going to eat you if you don't stop preparing the earnings reports and run away!");
            messageGenerator.AddIncomingMessage(208.0, "I don't have time for another one of your pranks.");
            messageGenerator.AddOutgoingMessage(55.0, "This is no prank, Tony! For real. Run now!");
            messageGenerator.AddIncomingMessage(89.0, "I know you're lying because you want me to \"own\" me by making me miss this deadline.");
            messageGenerator.AddOutgoingMessage(15.0, "No, Tony. I'm totally serious. He's distracted for a few seconds right now. Run now!");
            messageGenerator.AddIncomingMessage(24.0, "no");
            messageGenerator.AddOutgoingMessage(178.0, "Tony, I saw the wolf go into your office and come out with a piece of your shirt. Are you okay!?!");
            messageGenerator.AddOutgoingMessage(195.0, "Tony, NOOOOOOOOOOOOOOOOOOOOOOOOOO!");
            messageGenerator.AddOutgoingMessage(47.0, "Why didn't you listen to me? Why'd you have to go and get OWNED");
            messageGenerator.AddOutgoingMessage(8.0, "(by the wolf)");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.VictoriaWolfCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2009, 11, 9, 14, 45, 15);
            messageGenerator.AddIncomingMessage(0.0, "Why is Tony asking me if we're having an affair?");
            messageGenerator.AddOutgoingMessage(208.0, "Say we are. Just play along.");
            messageGenerator.AddIncomingMessage(55.0, "ok lol");
            messageGenerator.SetCurrentTime(2010, 8, 19, 11, 10, 04);
            messageGenerator.AddOutgoingMessage(0.0, "Hey your husband got eaten by a wolf. Sry");
            messageGenerator.AddIncomingMessage(39.0, "Darn! He was supposed to pick up the milk lol");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.WolfmanJackCell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2014, 10, 25, 11, 08, 01);
            {
                var audioAttachment = new Mock<IMessageAttachment>();
                audioAttachment.Setup(x => x.Type).Returns(AttachmentType.Audio);
                audioAttachment.Setup(x => x.Path).Returns( @"C:\fakepath\backup\056798632135464");
                audioAttachment.Setup(x => x.OriginalFilename).Returns("wolfman_howl.amr");
                messageGenerator.AddIncomingMessageWithAttachment(0.0, "Ooowooo! Wolfman Jack says someone's at the front door, baby!",
                                                                  audioAttachment.Object);
            }
            messageGenerator.AddOutgoingMessage(3.0, "That's the most wonderful thing I've ever heard!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.UnknownLawnmower);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 1, 4, 13, 32, 7);
            messageGenerator.AddIncomingMessage(0.0, "hey man I ran over my old phone with my lawnmower, so this is my new number -Ted Bremmer");
            messageGenerator.AddIncomingMessage(93.0, "also, could I borrow your lawnmower?");
            messageGenerator.AddOutgoingMessage(208.0, "You have the wrong number. I don't know a Ted Bremmer.");
            messageGenerator.AddIncomingMessage(3.0, "cmon, dude. don't play that game with me. I just need it for one afternoon.");
            messageGenerator.AddIncomingMessage(1083.0, "i don't believe this! i made you godfather to my son and you won't lend me your lawnmower?");
            messageGenerator.AddIncomingMessage(1892.0, "i'm disgusted that i was ever friends with you. I can't believe I used to tell people you were a generous guy.");
            messageGenerator.AddIncomingMessage(123.0, "don't ever speak to me again. you're dead to me.");
            messageGenerator.SetCurrentTime(2011, 1, 5, 11, 45, 26);
            messageGenerator.AddIncomingMessage(0.0, "there's gonna be a nice surprise for you when you get home from work, buddy.");
            messageGenerator.AddIncomingMessage(748.0, "yeah! that's right! I burned down your toolshed! hope you enjoy your precious lawnmower now!");
            messageGenerator.SetCurrentTime(2011, 1, 6, 18, 20, 13);
            messageGenerator.AddIncomingMessage(0.0, "whoops, looks like I've got the wrong number. the 3 and 6 are really close together on this phone. please disregard.");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.UnknownEagle);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 2, 7, 20, 38, 17);
            messageGenerator.AddIncomingMessage(0.0, "the eagle flies at noon");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            phoneNumberStripped = GetPhoneNumberStripped(DummyPhoneNumberId.UnknownGrahamBell);
            messageGenerator = new MockMessageGenerator(phoneNumberStripped, 2011, 3, 10, 13, 41, 17);
            messageGenerator.AddIncomingMessage(0.0, "Mr. Watson. Come here. I want to see you.");
            messageGenerator.AddOutgoingMessage(35.0, "Whoa! Your phone number is '1'?!?!");
            messageGenerator.AddIncomingMessage(74.0, "Well, I should expect so. It was my idea.");
            messageGenerator.AddOutgoingMessage(5.0, "the phone...?");
            messageGenerator.AddOutgoingMessage(12.0, "wait, you're not...");
            messageGenerator.AddIncomingMessage(28.0, "ya down with AGB?");
            messageGenerator.AddOutgoingMessage(29.0, "Yeah. You know me?");
            messageGenerator.AddIncomingMessage(28.0, "All will be clear to you very soon.");
            messageGenerator.AddOutgoingMessage(47.0, "Sweet!");
            InitMessageSet(dummyMessages, phoneNumberStripped, messageGenerator);

            chatRoomName = GetChatRoomInfo(DummyChatRoomId.ChatRoomA).ChatId;
            chatMessageGenerator = new MockChatMessageGenerator(chatRoomName, 2012, 2, 11, 13, 14, 28);
            chatMessageGenerator.AddOutgoingMessage(0.0, "Whatup gov't buddies?!?");
            chatMessageGenerator.AddIncomingMessage(12.0, GetPhoneNumber(DummyPhoneNumberId.ObamaCell), "Say word, buddy!");
            chatMessageGenerator.AddIncomingMessage(15.0, GetPhoneNumber(DummyPhoneNumberId.AnthonyWeinerCell), "What's crack-a-lackin!");
            InitMessageSet(dummyMessages, chatRoomName, chatMessageGenerator);

            chatRoomName = GetChatRoomInfo(DummyChatRoomId.ChatRoomB).ChatId;
            chatMessageGenerator = new MockChatMessageGenerator(chatRoomName, 2012, 2, 15, 00, 00, 00);
            chatMessageGenerator.AddOutgoingMessage(0.0, "Hey guys, sorry about letting Tony die...");
            InitMessageSet(dummyMessages, chatRoomName, chatMessageGenerator);

            chatRoomName = GetChatRoomInfo(DummyChatRoomId.ChatRoomC).ChatId;
            chatMessageGenerator = new MockChatMessageGenerator(chatRoomName, 2012, 2, 26, 18, 31, 05);
            chatMessageGenerator.AddOutgoingMessage(0.0, "Party tonight! 8pm my place!");
            chatMessageGenerator.AddIncomingMessage(12.0, GetPhoneNumber(DummyPhoneNumberId.ReliableLarryOffice), "I'll be there at 8:00:00 exactly!");
            chatMessageGenerator.AddIncomingMessage(18.0, GetPhoneNumber(DummyPhoneNumberId.ObamaCell), "I'm there. I'm rollin' deep, fyi.");
            chatMessageGenerator.AddIncomingMessage(35.0, GetPhoneNumber(DummyPhoneNumberId.HarryLooseTieCell), "Yay! PARTAAAAY!");
            chatMessageGenerator.AddIncomingMessage(12.0, GetPhoneNumber(DummyPhoneNumberId.AnthonyWeinerCell), "Will there be girls?");
            chatMessageGenerator.AddOutgoingMessage(18.0, "Gonna be off the hook!");
            InitMessageSet(dummyMessages, chatRoomName, chatMessageGenerator);

            chatRoomName = GetChatRoomInfo(DummyChatRoomId.ChatRoomD).ChatId;
            chatMessageGenerator = new MockChatMessageGenerator(chatRoomName, 2012, 10, 31, 21, 38, 05);
            chatMessageGenerator.AddOutgoingMessage(0.0, "Roll call!");
            chatMessageGenerator.AddIncomingMessage(12.0, GetPhoneNumber(DummyPhoneNumberId.HarryLooseTieCell), "H-Loose Tizzy in the hizzy!");
            chatMessageGenerator.AddIncomingMessage(18.0, GetPhoneNumber(DummyPhoneNumberId.UnknownLawnmower), "What what?");
            chatMessageGenerator.AddOutgoingMessage(0.0, "Nice work, gang!");
            InitMessageSet(dummyMessages, chatRoomName, chatMessageGenerator);

            foreach (DummyPhoneNumberId phoneNumberId in Enum.GetValues(typeof(DummyPhoneNumberId)))
            {
                phoneNumberStripped = GetPhoneNumberStripped(phoneNumberId);
                if (!dummyMessages.ContainsKey(phoneNumberStripped))
                {
                    dummyMessages[phoneNumberStripped] = new MessageSet(new List<TextMessage>());
                }
            }

            foreach (DummyChatRoomId chatId in Enum.GetValues(typeof(DummyChatRoomId)))
            {
                string chatRoomId = GetChatRoomInfo(chatId).ChatId;
                if (!dummyMessages.ContainsKey(chatRoomId))
                {
                    dummyMessages[chatRoomId] = new MessageSet(new List<TextMessage>());
                }
            }

            _DummyMessageSets = dummyMessages;
        }

        public static string GetPhoneNumber(DummyPhoneNumberId phoneNumberId)
        {
            return _DummyPhoneNumbers[(int)phoneNumberId];
        }

        public static string GetPhoneNumberStripped(DummyPhoneNumberId phoneNumberId)
        {
            return _DummyPhoneNumbersStripped[(int)phoneNumberId];
        }

        public static IContact GetContact(DummyPhoneNumberId phoneNumberId)
        {
            return _DummyContacts[(int)phoneNumberId];
        }

        public static List<Contact> GetContacts(DummyContactId contactId)
        {
            List<Contact> matchingContacts = new List<Contact>();
            foreach (Contact c in _DummyContacts)
            {
                if (c.ContactId == (long)contactId)
                {
                    matchingContacts.Add(c);
                }
            }

            return matchingContacts;
        }

        private static IContact GetContactByPhoneNumber(string phoneNumberValueToFind)
        {
            PhoneNumber phoneNumberToFind = new PhoneNumber(phoneNumberValueToFind);
            foreach (IContact contact in _DummyContacts)
            {
                foreach (IPhoneNumber phoneNumber in contact.PhoneNumbers)
                {
                    if (PhoneNumber.NumbersAreEquivalent(phoneNumberToFind, phoneNumber))
                    {
                        return contact;
                    }
                }
            }

            return null;
        }

        public static List<Contact> GetContacts(IEnumerable<DummyContactId> contactIds)
        {
            List<Contact> contacts = new List<Contact>();

            foreach (DummyContactId contactId in contactIds)
            {
                contacts.AddRange(GetContacts(contactId));
            }

            return contacts;
        }

        public static List<TextMessage> GetMessageSet(DummyPhoneNumberId setId)
        {
            string phoneNumberStripped = GetPhoneNumberStripped(setId);
            return _DummyMessageSets[phoneNumberStripped].MessageList;
        }

        public static List<TextMessage> GetMessageSet(DummyChatRoomId chatId)
        {
            string chatRoomName = GetChatRoomInfo(chatId).ChatId;
            return _DummyMessageSets[chatRoomName].MessageList;
        }

        public static List<TextMessage> GetMessageSet(DummyContactId contactId)
        {
            List<TextMessage> messages = new List<TextMessage>();
            
            IEnumerable<IContact> contacts = GetContacts(contactId);

            foreach (IContact contact in contacts)
            {
                foreach (PhoneNumber phoneNumber in contact.PhoneNumbers)
                {
                    string phoneNumberStripped = PhoneNumber.Strip(phoneNumber);
                    messages.AddRange(_DummyMessageSets[phoneNumberStripped]);
                }
            }

            return messages;
        }

        public static List<TextMessage> GetMessages(IEnumerable<DummyPhoneNumberId> messageSetIds)
        {
            List<TextMessage> messages = new List<TextMessage>();

            foreach (DummyPhoneNumberId messageSetId in messageSetIds)
            {
                messages.AddRange(GetMessageSet(messageSetId));
            }

            return messages;
        }

        public static List<TextMessage> GetMessages(IEnumerable<DummyChatRoomId> chatIds)
        {
            List<TextMessage> messages = new List<TextMessage>();

            foreach (DummyChatRoomId chatId in chatIds)
            {
                messages.AddRange(GetMessageSet(chatId));
            }

            return messages;
        }

        public static ChatRoomInformation GetChatRoomInfo(DummyChatRoomId chatRoomId)
        {
            return _DummyChatRoomInfoItems[(int)chatRoomId];
        }

        public static List<ChatRoomInformation> GetChatRoomInfoItems(IEnumerable<DummyChatRoomId> chatRoomIds)
        {
            List<ChatRoomInformation> chatRoomInfoItems = new List<ChatRoomInformation>();

            foreach (DummyChatRoomId chatRoomId in chatRoomIds)
            {
                chatRoomInfoItems.Add(GetChatRoomInfo(chatRoomId));
            }

            return chatRoomInfoItems;
        }

        public static int GetMessageCount(DummyPhoneNumberId setId)
        {
            string phoneNumberStripped = GetPhoneNumberStripped(setId);
            return _DummyMessageSets[phoneNumberStripped].Count;
        }

        public static int GetMessageCount(IEnumerable<DummyPhoneNumberId> setIds)
        {
            int messageCount = 0;

            foreach (DummyPhoneNumberId setId in setIds)
            {
                messageCount += GetMessageCount(setId);
            }

            return messageCount;
        }

        public static int GetPhoneNumberCount(IEnumerable<DummyContactId> contactIds)
        {
            int phoneNumberCount = 0;

            foreach (Contact contact in GetContacts(contactIds))
            {
                phoneNumberCount += contact.PhoneNumbers.Count;
            }

            return phoneNumberCount;
        }

        public static ConversationManager GetConversationManager(IEnumerable<DummyContactId> dummyContactIds,
                                                                 IEnumerable<DummyPhoneNumberId> messageSetIds,
                                                                 IEnumerable<DummyChatRoomId> chatRoomIds,
                                                                 ILoadingProgressCallback progressCallback)
        {
            List<Contact> contacts = DummyConversationDataGenerator.GetContacts(dummyContactIds);

            List<TextMessage> messages = DummyConversationDataGenerator.GetMessages(messageSetIds);

            messages.AddRange(DummyConversationDataGenerator.GetMessages(chatRoomIds));

            List<ChatRoomInformation> chatInfoItems = DummyConversationDataGenerator.GetChatRoomInfoItems(chatRoomIds);

            List<MessageAttachment> attachments = new List<MessageAttachment>();

            return new ConversationManager(contacts, messages, chatInfoItems, attachments, progressCallback);
        }

        public static ConversationManager GetConversationManager(IEnumerable<DummyContactId> dummyContactIds,
                                                                 IEnumerable<DummyPhoneNumberId> messageSetIds,
                                                                 ILoadingProgressCallback progressCallback)
        {
            DummyChatRoomId[] dummyChatIds = { };

            return GetConversationManager(dummyContactIds, messageSetIds, dummyChatIds, progressCallback);
        }

        public static ConversationManager GetConversationManagerEmpty()
        {
            List<Contact> contacts = new List<Contact>();
            List<TextMessage> messages = new List<TextMessage>();
            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();
            List<MessageAttachment> attachments = new List<MessageAttachment>();

            return new ConversationManager(contacts, messages, chatInfoItems, attachments, null);
        }

        private static Contact GetAssociatedContact(DummyPhoneNumberId phoneNumberId)
        {
            return _DummyContacts[(int)phoneNumberId];
        }

        public static IConversation GetSingleConversation(DummyPhoneNumberId messageSetId)
        {
            Contact contact = GetAssociatedContact(messageSetId);
            List<Contact> contacts = new List<Contact>(1);
            if (contact != null)
            {
                contacts.Add(contact);
            }
            List<TextMessage> messages = DummyConversationDataGenerator.GetMessageSet(messageSetId);
            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();
            List<MessageAttachment> attachments = new List<MessageAttachment>();

            ConversationManager conversationManager = new ConversationManager(contacts, messages, chatInfoItems, attachments, null);

            return conversationManager.GetConversation(0);
        }

        public static IConversation GetMergedConversation(DummyContactId contactId)
        {
            List<Contact> contacts = GetContacts(contactId);
            
            IEnumerable<TextMessage> messages = GetMessageSet(contactId);

            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();

            List<MessageAttachment> attachments = new List<MessageAttachment>();

            ConversationManager conversationManager = new ConversationManager(contacts, messages, chatInfoItems, attachments, null);

            MergingConversationManager megingConversationManager = new MergingConversationManager(conversationManager, null);

            return megingConversationManager.GetConversation(0);
        }

        public static IConversation GetChatConversation(DummyChatRoomId chatRoomId)
        {
            List<IContact> contacts = new List<IContact>();
            List<TextMessage> messages = DummyConversationDataGenerator.GetMessageSet(chatRoomId);
            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();
            ChatRoomInformation chatRoomInfo = GetChatRoomInfo(chatRoomId);
            chatInfoItems.Add(chatRoomInfo);
            List<MessageAttachment> attachments = new List<MessageAttachment>();

            foreach (string phoneNumberValue in chatRoomInfo.Participants)
            {
                IContact associatedContact = GetContactByPhoneNumber(phoneNumberValue);
                if (associatedContact != null)
                {
                    contacts.Add(associatedContact);
                }
            }

            ConversationManager conversationManager = new ConversationManager(contacts, messages, chatInfoItems, attachments, null);
            foreach (IConversation conversation in conversationManager)
            {
                if (conversation.MessageCount > 0)
                {
                    return conversation;
                }
            }

            throw new ArgumentException("Shouldn't reach here!");
        }
    }
}
