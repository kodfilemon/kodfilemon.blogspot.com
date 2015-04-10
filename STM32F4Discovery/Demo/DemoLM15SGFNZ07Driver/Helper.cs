////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.SPOT;

namespace DemoLM15SGFNZ07Driver
{
    public abstract class Test 
    {
        public string Name;
		public bool   Pass;
        
        protected Test( string comment )
        {
            Name    = ToString(); 
			Pass    = false;

			Debug.Print( "Test        : " + Name                                      );
			Debug.Print( "Test Comment: " + comment                                   );
        }

        protected Test()
        {
			Name    = ToString(); 
			Pass    = false;

			Debug.Print( "Test        : " + Name                                      );
			Debug.Print( "Test Comment: N/A"                                          );
		}

		public void Finished()
		{
		    Debug.Print("Test " + (Pass ? "Passed" : "Failed"));
			Debug.Print("==================================================================");
		}        

		public abstract void Run();
		
		protected internal void UnexpectedException( Exception e )
        {
            Pass = false;
			Debug.Print( "Test        : " + Name   + " Unexpected Exception occurred." );                                          
            Debug.Print( e.Message                                                    );
            Debug.Print( "Exception Stack Trace: "                                    );
            Debug.Print( e.StackTrace                                                 );   
        }

        protected internal void UnexpectedBehavior()
        {
			Pass = false;
            Debug.Print( "Test        : " + Name   + " Unexpected Behavior."            );
		}
    }

    public class TestSuite
    {
		private int _testsPassed;
		private int _testsTotal;

        public TestSuite()
        {
			Debug.Print("==================================================================");
			_testsPassed = 0;
			_testsTotal = 0;
        }
		public void Finished()
		{
			Debug.Print("Total Tests Passed =   " + _testsPassed);
			Debug.Print("Total Tests Failed =   " + (_testsTotal - _testsPassed));
			Debug.Print("Total Tests Executed = " + _testsTotal );
			Debug.Print("==================================================================");
		}

		public void RunTest( Test testToRun )
		{ 
			_testsTotal++;
			try
			{
				testToRun.Run();
			}
			catch(Exception e)
			{
				testToRun.Pass = false;
				Debug.Print( "Unexpected Exception" + e.StackTrace );    
			}

			if( testToRun.Pass )
				++_testsPassed;

			testToRun.Finished();
		}
    }
}
