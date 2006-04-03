using System;
using System.Collections.Generic;
using System.Text;
using ErrorReportExtractor.ErrorReportsDataSetTableAdapters;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using ErrorReportExtractor.Properties;

namespace ErrorReportExtractor
{
    class Storage : ErrorReportExtractor.IStorage
    {
        public Storage(IProgressCallback callback)
        {
            this.callback = callback;
        }
        //public void Store(MailItem mailItem)
        //{
        //}

        public void Store(IEnumerable<IErrorReport> items)
        {
            QueriesTableAdapter itemsAdapter = new QueriesTableAdapter();
            StackTraceLinesTableAdapter linesAdapter = new StackTraceLinesTableAdapter();

            foreach (IErrorReport item in items)
            {
                //SqlTransaction trans = null;
                try
                {
                    if (callback.VerboseMode)
                    {
                        this.callback.Verbose("Inserting item with ID {0}, timestamp {1}", item.ID, item.ReceivedTime);
                    }
                    else
                    {
                        this.callback.Progress();
                    }
                    
                    //trans = itemsAdapter.BeginTransaction(conn);
                    //linesAdapter.JoinTransaction(trans);

                    int inserted = (int)
                    itemsAdapter.ImportErrorItem(item.ID, item.ReceivedTime, item.SenderEmail, item.SenderName, item.Body, item.Subject,
                        item.ExceptionType, "", "", item.MajorVersion, item.MinorVersion, item.PatchVersion, 
                        item.Revision, item.RepliedTo);

                    if (inserted == 0)
                    {
                        callback.Warning("Item with id {0} already exists in base", item.ID);
                        continue;
                    }

                    foreach (IStackTraceItem stItem in item.StackTrace)
                    {
                        linesAdapter.Insert(item.ID, stItem.MethodName, stItem.Parameters, stItem.Filename, stItem.LineNumber, stItem.SequenceNumber);
                    }

                    //trans.Commit();
                }
                catch (Exception )
                {
                    //if (trans != null)
                    //{
                    //    trans.Rollback();
                    //}   
                    throw;
                }
            }
        }

        public DateTime GetLastDate()
        {
            return DateTime.Now;
        }

        public IEnumerable<IErrorReport> GetAllReports()
        {
            //ErrorReportsDataSet ds = new ErrorReportsDataSet();
            //SqlDataAdapter adapter = new SqlDataAdapter( 
            //    "SELECT * FROM ErrorReportItems WHERE RepliedTo = 0 ORDER BY ReceivedTime ASC",
            //    Settings.Default.ErrorReportsConnectionString );
            //adapter.Fill( ds.ErrorReportItems );
            ErrorReportItemsTableAdapter adapter = new ErrorReportItemsTableAdapter();
            ErrorReportsDataSet.ErrorReportItemsDataTable table = adapter.GetAll();
            //ErrorReportItemsTableAdapter adapter = new ErrorReportItemsTableAdapter();
            this.callback.Info( "Got {0} error reports from the database.", table.Count );
            foreach ( ErrorReportsDataSet.ErrorReportItemsRow row in table)
            {
                ErrorReport report = new ErrorReport( row.ID, row.Subject, row.Body, row.SubmitterEmail, row.SubmitterName,
                    row.ReceivedTime );
                report.RepliedTo = row.RepliedTo;
                yield return report;
            }
            
        }

        private IProgressCallback callback;

        #region IStorage Members


        public void AnswerReport( IErrorReport report, string replyText )
        {
            QueriesTableAdapter adapter = new QueriesTableAdapter();
            int count = (int)adapter.ReplyToReport( report.ID, replyText, Settings.Default.SenderEmail, report.SenderEmail,
                Settings.Default.SenderName, report.SenderName, null, null );
            if ( count != 1 )
            {
                callback.Error( "Attempting to mark message {0} as replied to, but {1} records in the database matched that ID",
                    report.ID, count );
            }
            else
            {
                report.RepliedTo = true;
            }
        }

        #endregion
    }
}
