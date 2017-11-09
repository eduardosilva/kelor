using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelor.Commands
{
    public class CopyFolder : KelorCommand
    {
        private IEnumerable<KelorCommandParameter> parameteres;

        private string sourceFolder;
        private string target;
        private IEnumerable<string> exceptFiles;
        private bool onlyContent;

        public CopyFolder()
        {
            parameteres = new[]
            {
                new KelorCommandParameter { Key = Constants.SOURCE, Description = "Folder to copy", Required = true },
                new KelorCommandParameter { Key = Constants.TO, Description = "Folder destination", Required = true },
                new KelorCommandParameter { Key = Constants.EXCEPT_FILES_RULES, Description = "Files cannot be copied", Required = false },
                new KelorCommandParameter { Key = Constants.ONLY_CONTENT, Description = "Indicate only folder content must be copied", NotRequiredAValue = true }
            };
        }

        public override string Code
        {
            get { return "copyfolder"; }
        }

        public override string Description
        {
            get { return "Copy folder or only its content"; }
        }

        public override IEnumerable<IKelorCommandParameter> Parameters
        {
            get { return parameteres; }
        }

        protected override void Execute()
        {
            sourceFolder = parameteres.FirstOrDefault(p => p.Key == Constants.SOURCE).Value;
            target = parameteres.FirstOrDefault(p => p.Key == Constants.TO).Value;
            onlyContent = parameteres.Any(p => p.Key == Constants.ONLY_CONTENT && p.Declared);

            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine("Source folder does not exists");
                return;
            }

            var exceptFilesRule = parameteres.FirstOrDefault(p => p.Key == Constants.EXCEPT_FILES_RULES).Value;
            exceptFiles = ExtractExceptFilesRules(exceptFilesRule);

            if (!onlyContent) {
                var targetFolderName = Path.GetFileNameWithoutExtension(sourceFolder);
                target = Path.Combine(target, targetFolderName);
            }

            CopyDirectoryAndFiles(sourceFolder, target);
        }

        private IEnumerable<string> ExtractExceptFilesRules(string exceptFilesRule)
        {
            var result = new List<string>();

            if (String.IsNullOrWhiteSpace(exceptFilesRule))
                return result;

            result = exceptFilesRule.Split(',').Where(s => !String.IsNullOrWhiteSpace(s))
                                               .Select(s => s.Trim())
                                               .Select(s =>
                                               {
                                                   var r = s.Replace("[", "")
                                                            .Replace("]", "");

                                                   if (r.Contains("*"))
                                                       r = r.Replace("*", "");

                                                   return r;
                                               }).ToList();

            foreach (var r in result)
            {
                Console.WriteLine("Exclude file: ", r);
            }

            return result;
        }

        private void CopyDirectoryAndFiles(string source, string to)
        {
            var directories = Directory.GetDirectories(source).Select(d => new DirectoryInfo(d));
            if (!directories.Any())
                CopyFiles(source, to);

            foreach (var directory in directories)
            {
                var toDirectory = Path.Combine(to, directory.Name);
                if (!Directory.Exists(toDirectory))
                    Directory.CreateDirectory(toDirectory);

                CopyDirectoryAndFiles(directory.FullName, toDirectory);
            }

            CopyFiles(source, to);
        }

        private void CopyFiles(string source, string to)
        {
            var files = Directory.GetFiles(source).Select(f => new FileInfo(f))
                                                  .Where(f => !exceptFiles.Any(e => f.Name.Contains(e)));

            foreach (var file in files)
            {
                Console.WriteLine($"Copying file { file }");
                File.Copy(file.FullName, Path.Combine(to, file.Name), overwrite: true);
            }
        }
    }
}
