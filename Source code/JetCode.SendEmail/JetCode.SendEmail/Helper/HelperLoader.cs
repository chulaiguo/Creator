using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JetCode.SendEmail.Helper
{
    public class HelperLoader
    {
        public SortedList<string, string> LoadEmailAddress(string fileName)
        {
            try
            {
                DataSet dataSet;
                if (fileName.ToLower().EndsWith("csv"))
                {
                    dataSet = this.LoadFromCSV(fileName);
                }
                else
                {
                    dataSet = this.LoadFromExcel(fileName);
                }

                if (dataSet == null)
                    return null;

                return this.GetEmailList(dataSet);
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex.Message);
            }

            return null;
        }

        private DataSet LoadFromCSV(string fileName)
        {
            try
            {
                DataTable table = new DataTable();
                table.TableName = "CSV";
                string[] lines = File.ReadAllLines(fileName);
                foreach (string item in lines)
                {
                    if (item.Length == 0)
                        continue;

                    string[] splits = item.Split(',');
                    int colDiff = splits.Length - table.Columns.Count;
                    for (int i = 0; i < colDiff; i++)
                    {
                        table.Columns.Add(new DataColumn());
                    }

                    table.LoadDataRow(splits, LoadOption.OverwriteChanges);
                }

                if (table.Rows.Count == 0)
                    return null;

                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(table);
                return dataSet;
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex.Message);
            }

            return null;
        }

        private DataSet LoadFromExcel(string fileName)
        {
            try
            {
                HelperExcel excel = new HelperExcel();
                DataSet dataSet = excel.LoadIntoDataSet(fileName, false);
                if (dataSet.Tables.Count == 0)
                    return null;

                return dataSet;
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex.Message);
            }

            return null;
        }

        private SortedList<string, string> GetEmailList(DataSet dataSet)
        {
            SortedList<string, string> retList = new SortedList<string, string>();
            foreach (DataTable table in dataSet.Tables)
            {
                SortedList<string, string> list = this.GetEmailList(table);
                foreach (KeyValuePair<string, string> pair in list)
                {
                    if (retList.ContainsKey(pair.Key))
                        continue;
                    retList.Add(pair.Key, pair.Value);
                }
            }

            return retList;
        }

        private SortedList<string, string> GetEmailList(DataTable table)
        {
            SortedList<string, string> retList = new SortedList<string, string>();
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    string emails = row[i].ToString();
                    emails = emails.Trim().Trim('\"');
                    string[] splits = emails.Split(';');
                    foreach (string item in splits)
                    {
                        string email = item.Trim().ToLower();
                        if (retList.ContainsKey(email))
                            continue;

                        if (!HelperEmail.IsValidEmail(email))
                            continue;

                        retList.Add(email, email);
                    }
                }
            }

            return retList;
        }

        private void ShowErrorMessage(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
