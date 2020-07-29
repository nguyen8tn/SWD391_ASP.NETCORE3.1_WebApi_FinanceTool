using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Data
{
    public class GoogleSheetData
    {
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        private const string SpreadsheetId = "1etN1Y2Qb-76c_IQjDH125zwPfQrSSgb-AodNsfnSFBM";
        private const string GoogleCredentialsFileName = "google-api.json";
        static readonly string sheet = "sheet1";
        static SheetsService service;

        public static void CreateService()
        {
            GoogleCredential credential;

            using (var stream =
                new FileStream(GoogleCredentialsFileName, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public static void ReadEntries()
        {
            var range = $"{sheet}!A:B";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var col in values)
                {
                    Console.WriteLine("{0}, {1}", col[1], col[2]);
                }
            }
        }
        public static void AddEntries(string formulaName)
        {
            var range = $"{sheet}!A:B";
            var valueRange = new ValueRange();
            var objectList = new List<object>() { DateTime.Today , formulaName };
            valueRange.Values = new List<IList<object>> { objectList };
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }
    }
}
