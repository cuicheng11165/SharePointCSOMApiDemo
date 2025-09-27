using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using File = Microsoft.SharePoint.Client.File;

namespace CSOM_File_Add_Test
{
    internal static class FileAddScenarios
    {
        internal static void UpdateManagedMetadataDefaultValue()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            using ClientContext context = new ClientContext("https://bigapp.sharepoint.com/teams/Teams202504221153");

            var list = context.Web.Lists.GetByTitle("m22");
            var column = list.Fields.GetByTitle("m1");

            context.Load(column);
            context.ExecuteQuery();

            var termName = "C";
            var termId = "23f5a117-458e-44fa-ac24-ff1fe1926054";

            var session = TaxonomySession.GetTaxonomySession(context);

            var targetTerm = session.GetTerm(new Guid(termId));
            context.Load(targetTerm);
            context.ExecuteQuery();

            TaxonomyField mmColumn = context.CastTo<TaxonomyField>(column);

            var value = new TaxonomyFieldValueCollection(context, $"-1;#{termName}|{termId}", mmColumn);

            ClientResult<string> defaultValue = mmColumn.GetValidatedString(value);

            context.ExecuteQuery();

            var defaultValueString = string.Empty;
            if (!string.IsNullOrEmpty(defaultValue.Value))
            {
                defaultValueString = defaultValue.Value;
            }
            else
            {
                defaultValueString = $"-1;#{termName}|{termId}";
            }

            column.DefaultValue = defaultValueString;

            column.UpdateAndPushChanges(true);

            context.ExecuteQuery();
        }

        internal static void Write(Action testDelegate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            testDelegate.Invoke();
            stopwatch.Stop();
            Console.WriteLine($"TimeUsed :{stopwatch.ElapsedMilliseconds}");
        }

        internal static void AddFileWithBytes()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            Folder folder = context.Web.GetFolderByServerRelativeUrl("https://cnblogtest.sharepoint.com/Documents%20Test");

            var newAddedFile = folder.Files.Add(new FileCreationInformation
            {
                Url = "AddFileWithBytes.txt",
                Overwrite = true,
                Content = Encoding.UTF8.GetBytes("TestDocumentContent")
            });

            context.Load(newAddedFile);
            context.ExecuteQuery();
        }

        internal static void AddFileWithStream()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            Folder folder = context.Web.GetFolderByServerRelativeUrl("https://cnblogtest.sharepoint.com/Documents%20Test");

            var newAddedFile = folder.Files.Add(new FileCreationInformation
            {
                Url = "AddFileWithStream.txt",
                Overwrite = true,
                ContentStream = new MemoryStream(Encoding.UTF8.GetBytes("TestDocumentContent"))
            });

            context.Load(newAddedFile);
            context.ExecuteQuery();
        }

        internal static void AddLargeFileWithStream()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            Folder folder = context.Web.GetFolderByServerRelativeUrl("https://cnblogtest.sharepoint.com/Documents%20Test");

            using FileStream fs = new FileStream("d:\\TestObject.rar", FileMode.Open);

            var newAddedFile = folder.Files.Add(new FileCreationInformation
            {
                Url = "AddFileWithStreamLarge.rar",
                Overwrite = true,
                ContentStream = fs
            });

            context.Load(newAddedFile);
            context.ExecuteQuery();
        }

        internal static void AddFileWithSaveBinaryDirect()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            context.ExecuteQuery();
        }

        internal static void AddLargeFileWithSaveBinaryDirect()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            using FileStream fs = new FileStream("d:\\TestObject.rar", FileMode.Open);

            // File.SaveBinaryDirect(context, "/Documents%20Test/AddFileWithSaveBinaryDirectLarge.rar", fs, true);

            context.ExecuteQuery();
        }

        internal static void AddFileWithSaveBytes()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            var web = context.Web;

            var file = web.GetFileByServerRelativeUrl("/Documents%20Test/AddFileWithSaveBytes.txt");
            file.SaveBinary(new FileSaveBinaryInformation
            {
                Content = Encoding.UTF8.GetBytes("TestDocumentContent")
            });

            context.ExecuteQuery();
        }

        internal static void AddFileWithSaveStream()
        {
            using ClientContext context = new ClientContext("https://cnblogtest.sharepoint.com");

            var web = context.Web;

            var file = web.GetFileByServerRelativeUrl("/Documents%20Test/AddFileWithSaveStream.txt");
            file.SaveBinary(new FileSaveBinaryInformation
            {
                ContentStream = new MemoryStream(Encoding.UTF8.GetBytes("TestDocumentContent"))
            });

            context.ExecuteQuery();
        }

        internal static void AddFileWithContinueUpload()
        {
            string fileName = "C:\\Users\\chengcui\\Documents\\DataLakeReader.xlsx";

            using ClientContext context = new ClientContext("https://bigapp.sharepoint.com/sites/SimmonDynamicAutoTest20220908021450");

            ClientResult<long>? bytesUploaded = null;

            File? uploadFile = null;
            Guid uploadId = Guid.NewGuid();

            using FileStream fs = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using BinaryReader br = new BinaryReader(fs);

            byte[] buffer = new byte[1024 * 1024];
            byte[]? lastBuffer = null;
            long fileoffset = 0;
            long totalBytesRead = 0;
            int bytesRead;
            bool first = true;
            bool last = false;

            while ((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == fs.Length)
                {
                    last = true;
                    lastBuffer = new byte[bytesRead];
                    Array.Copy(buffer, 0, lastBuffer, 0, bytesRead);
                }

                if (first)
                {
                    using MemoryStream contentStream = new MemoryStream();

                    FileCreationInformation fileInfo = new FileCreationInformation
                    {
                        ContentStream = contentStream,
                        Url = Path.GetFileName(fileName),
                        Overwrite = true
                    };

                    Folder folder = context.Web.GetFolderByServerRelativeUrl("/sites/SimmonDynamicAutoTest20220908021450/shared documents");

                    uploadFile = folder.Files.Add(fileInfo);

                    using MemoryStream s = new MemoryStream(buffer, 0, bytesRead);
                    {
                        bytesUploaded = uploadFile.StartUpload(uploadId, s);
                        context.ExecuteQuery();
                        fileoffset = bytesUploaded.Value;
                        Console.WriteLine($"fileoffset:{fileoffset}");
                    }

                    first = false;
                }
                else
                {
                    uploadFile = context.Web.GetFileByServerRelativeUrl("/sites/Test1/Test Library" + Path.AltDirectorySeparatorChar + Path.GetFileName(fileName));

                    if (last)
                    {
                        using MemoryStream s = new MemoryStream(lastBuffer!);
                        {
                            uploadFile = uploadFile.FinishUpload(uploadId, fileoffset, s);
                            context.ExecuteQuery();
                            Console.WriteLine($"fileoffset:{fileoffset}");
                            break;
                        }
                    }
                    else
                    {
                        using MemoryStream s = new MemoryStream(buffer, 0, bytesRead);
                        {
                            bytesUploaded = uploadFile.ContinueUpload(uploadId, fileoffset, s);
                            context.ExecuteQuery();
                            fileoffset = bytesUploaded.Value;
                            Console.WriteLine($"fileoffset:{fileoffset}");
                        }
                    }
                }
            }

            uploadFile = context.Web.GetFileByServerRelativeUrl("/sites/Test1/Test Library" + Path.AltDirectorySeparatorChar + Path.GetFileName(fileName));

            ConditionalScope conditionScope = new ConditionalScope(context, () => uploadFile.Exists, true);
            using (conditionScope.StartScope())
            {
                using (conditionScope.StartIfTrue())
                {
                    context.Load(uploadFile);
                    context.Load(uploadFile.ListItemAllFields);
                }
            }

            context.ExecuteQuery();
        }
    }
}
