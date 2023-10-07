using System;
using System.IO;
using System.Reflection.Metadata;


namespace CreateNodeProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nome do Projeto: ");
            string userInputProjectName = Console.ReadLine();

            Console.WriteLine("Deseja Iniciar com TypeScript? (Y:N): ");
            string userInputProjectTypeScript = Console.ReadLine();

            string fileType;

            if(userInputProjectTypeScript.ToLower() == "Y".ToLower())
            {
                fileType = "ts";
            } else
            {
                fileType = "js";
            }

            string projectPath = @$"C:\Users\PC\Desktop\{userInputProjectName}";
            
            try
            {
                if(Directory.Exists(projectPath))
                {
                    Console.WriteLine("Pasta já existe");

                    return;
                }

                Directory.CreateDirectory(projectPath);

                ExecuteCommand("npm init -y", projectPath);

                bool packageJsonExists = false;

                while(!packageJsonExists)
                {
                    if(File.Exists($@"{projectPath}\package.json"))
                    {
                        if(userInputProjectTypeScript.ToLower() == "Y".ToLower())
                        {
                            ExecuteCommand("npx tsc --init", projectPath);
                        }

                        ExecuteCommand("npm install express dotenv pg reflect-metadata typeorm", projectPath);
                        // ExecuteCommand("npm install -D typescript ts-node-dev @types/express @types/node", projectPath);
                        packageJsonExists = true;
                    }
                }

                Directory.CreateDirectory($@"{projectPath}\src");
                Directory.CreateDirectory($@"{projectPath}\src\controllers");
                Directory.CreateDirectory($@"{projectPath}\src\services");
                Directory.CreateDirectory($@"{projectPath}\src\routes");

                CreateFileObject cfo01 = new CreateFileObject(projectPath, ".gitignore", FileContent.GitIgnoreContent());
                CreateFileObject cfo02 = new CreateFileObject(projectPath, ".env", FileContent.DotEnvContent());
                CreateFileObject cfo03 = new CreateFileObject($@"{projectPath}\src", $"app.{fileType}", FileContent.AppContent());
                CreateFileObject cfo04 = new CreateFileObject($@"{projectPath}\src", $"server.{fileType}", FileContent.ServerContent());
                CreateFileObject cfo05 = new CreateFileObject($@"{projectPath}\src", $"data-source.{fileType}", FileContent.DataSourceContent());
                CreateFileObject cfo06 = new CreateFileObject($@"{projectPath}\src\controllers", $"user.controller.{fileType}", FileContent.UserControllerContent());
                CreateFileObject cfo07 = new CreateFileObject($@"{projectPath}\src\services", $"user.service.{fileType}", FileContent.UserServiceContent());
                CreateFileObject cfo08 = new CreateFileObject($@"{projectPath}\src\routes", $"index.{fileType}", FileContent.IndexRouteContent());
                CreateFileObject cfo09 = new CreateFileObject($@"{projectPath}\src\routes", $"user.route.{fileType}", FileContent.UserRouteContent());

                CreateFileObject[] allFiles = new CreateFileObject[9] { cfo01, cfo02, cfo03, cfo04, cfo05, cfo06, cfo07, cfo08, cfo09 };

                foreach (CreateFileObject current in allFiles)
                {
                    File.WriteAllText(Path.Combine(current.path, current.filename), current.content);
                }

                // MANIPULAR O ARQUIVO TSCONFIG.JSON, E DESCOMENTAR ALGUMAS CONFIGURAÇÕES

                string packageJsonMainContent = FileContent.PackageJsonMainContent(fileType);
                string packageJsonScriptsContent = FileContent.PackageJsonScriptsContent(fileType);

                ReplaceContent(projectPath, "\"main\": \"index.js\"", packageJsonMainContent);
                ReplaceContent(projectPath, "\"test\": \"echo \\\"Error: no test specified\\\" && exit 1\"", packageJsonScriptsContent);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        internal static void ExecuteCommand(string command, string projectPath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WorkingDirectory = projectPath;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/k {command}";
            process.StartInfo = startInfo;
            process.Start();
        }

        internal static void ReplaceContent(string projectPath, string contentToBeReplaced, string newContent)
        {
            string packageJsonPath = $@"{projectPath}\package.json";

            string packageJsonText = File.ReadAllText(packageJsonPath);
            packageJsonText = packageJsonText.Replace(contentToBeReplaced, newContent);
            File.WriteAllText(packageJsonPath, packageJsonText);
        }
    }
}