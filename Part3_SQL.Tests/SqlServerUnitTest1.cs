using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Part3_SQL.Tests
{
    [TestClass()]
    public class SqlServerUnitTest1 : SqlDatabaseTestClass
    {

        public SqlServerUnitTest1()
        {
            InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup()
        {
            base.CleanupTest();
        }

        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spGetFiveRecentResponseTextTest_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlServerUnitTest1));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition rowCountCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spInsert_server_response_logTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spPopulateData_server_response_logTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExecutionTimeCondition executionTimeCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExecutionTimeCondition executionTimeCondition2;
            this.dbo_spGetFiveRecentResponseTextTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spInsert_server_response_logTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spPopulateData_server_response_logTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            dbo_spGetFiveRecentResponseTextTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            rowCountCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            dbo_spInsert_server_response_logTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            dbo_spPopulateData_server_response_logTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            executionTimeCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExecutionTimeCondition();
            executionTimeCondition2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExecutionTimeCondition();
            // 
            // dbo_spGetFiveRecentResponseTextTest_TestAction
            // 
            dbo_spGetFiveRecentResponseTextTest_TestAction.Conditions.Add(rowCountCondition1);
            resources.ApplyResources(dbo_spGetFiveRecentResponseTextTest_TestAction, "dbo_spGetFiveRecentResponseTextTest_TestAction");
            // 
            // rowCountCondition1
            // 
            rowCountCondition1.Enabled = true;
            rowCountCondition1.Name = "rowCountCondition1";
            rowCountCondition1.ResultSet = 1;
            rowCountCondition1.RowCount = 5;
            // 
            // dbo_spInsert_server_response_logTest_TestAction
            // 
            dbo_spInsert_server_response_logTest_TestAction.Conditions.Add(executionTimeCondition2);
            resources.ApplyResources(dbo_spInsert_server_response_logTest_TestAction, "dbo_spInsert_server_response_logTest_TestAction");
            // 
            // dbo_spPopulateData_server_response_logTest_TestAction
            // 
            dbo_spPopulateData_server_response_logTest_TestAction.Conditions.Add(executionTimeCondition1);
            resources.ApplyResources(dbo_spPopulateData_server_response_logTest_TestAction, "dbo_spPopulateData_server_response_logTest_TestAction");
            // 
            // executionTimeCondition1
            // 
            executionTimeCondition1.Enabled = true;
            executionTimeCondition1.ExecutionTime = System.TimeSpan.Parse("00:00:30");
            executionTimeCondition1.Name = "executionTimeCondition1";
            // 
            // dbo_spGetFiveRecentResponseTextTestData
            // 
            this.dbo_spGetFiveRecentResponseTextTestData.PosttestAction = null;
            this.dbo_spGetFiveRecentResponseTextTestData.PretestAction = null;
            this.dbo_spGetFiveRecentResponseTextTestData.TestAction = dbo_spGetFiveRecentResponseTextTest_TestAction;
            // 
            // dbo_spInsert_server_response_logTestData
            // 
            this.dbo_spInsert_server_response_logTestData.PosttestAction = null;
            this.dbo_spInsert_server_response_logTestData.PretestAction = null;
            this.dbo_spInsert_server_response_logTestData.TestAction = dbo_spInsert_server_response_logTest_TestAction;
            // 
            // dbo_spPopulateData_server_response_logTestData
            // 
            this.dbo_spPopulateData_server_response_logTestData.PosttestAction = null;
            this.dbo_spPopulateData_server_response_logTestData.PretestAction = null;
            this.dbo_spPopulateData_server_response_logTestData.TestAction = dbo_spPopulateData_server_response_logTest_TestAction;
            // 
            // executionTimeCondition2
            // 
            executionTimeCondition2.Enabled = true;
            executionTimeCondition2.ExecutionTime = System.TimeSpan.Parse("00:00:01");
            executionTimeCondition2.Name = "executionTimeCondition2";
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion

        [TestMethod()]
        public void dbo_spPopulateData_server_response_logTest()
        {
            //Need to run this first to populate data
            SqlDatabaseTestActions testActions = this.dbo_spPopulateData_server_response_logTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }
        [TestMethod()]
        public void dbo_spGetFiveRecentResponseTextTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spGetFiveRecentResponseTextTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spInsert_server_response_logTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spInsert_server_response_logTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        private SqlDatabaseTestActions dbo_spPopulateData_server_response_logTestData;
        private SqlDatabaseTestActions dbo_spGetFiveRecentResponseTextTestData;
        private SqlDatabaseTestActions dbo_spInsert_server_response_logTestData;
    }
}
