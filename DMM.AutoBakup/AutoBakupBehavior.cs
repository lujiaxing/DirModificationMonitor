using Microsoft.Win32;
using DMM.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM.Behaviors.AutoBakup
{
    /// <summary>
    /// 自动备份处理
    /// </summary>
    public class AutoBakupBehavior: IBehavior
    {
        
        /// <summary>
        /// 执行自动备份.
        /// 
        /// 1. 读取文件并解压缩.
        /// 2. 根据压缩包内的文件结构进行增量备份.
        /// 3. 覆盖文件.
        /// 
        /// </summary>
        public OperationResult DoAction(string path, Dictionary<string, string> parameters, System.IO.FileSystemEventArgs modificationArg)
        {
            var processReturnCode = 0;
            var ext = Path.GetExtension(modificationArg.Name).ToLower();

            if (modificationArg.ChangeType != WatcherChangeTypes.Changed && modificationArg.ChangeType != WatcherChangeTypes.Created)
                return OperationResult.DoNothing;

            if (ext != ".rar" && ext != ".zip")
                return OperationResult.DoNothing;

            Logger.Instance.WriteInfoLog("准备处理: " + path + " 发生的变更.");

            String extactProgram = parameters["ExtactProgram"]; //解压缩程序路径
            String extactProgramArg = parameters["ExtactProgramArg"]; //解压缩程序执行参数
            String extactedPath = parameters["ExtactPath"]; //解压目标地址
            String webSiteBasePath = parameters["WebSiteBasePath"]; //目标基准目录
            String bakupBasePath = parameters["BakupBasePath"]; //备份基准目录


            //标准化路径格式(去掉路径最后的 "\", 以便后续处理)
            extactedPath = extactedPath.EndsWith("\\") ? extactedPath.Remove(extactedPath.Length - 1, 1) : extactedPath;
            webSiteBasePath = webSiteBasePath.EndsWith("\\") ? webSiteBasePath.Remove(webSiteBasePath.Length - 1, 1) : webSiteBasePath;
            bakupBasePath = bakupBasePath.EndsWith("\\") ? bakupBasePath.Remove(bakupBasePath.Length - 1, 1) : bakupBasePath;

            Logger.Instance.WriteInfoLog(
                String.Format("\nExtactProgram = {0}\nExtactProgramArg = {1}\nExtactPath = {2}" +
                "\nWebSiteBasePath = {3}\nBakupBasePath = {4}",
                extactProgram, extactProgramArg, extactedPath, webSiteBasePath, bakupBasePath));

            //X:\xxx\xxx\20140722180622_123\
            //将压缩包单独解压到一个路径下, 后续操作以此为准.
            String bakBathDirName =
                DateTime.Now.ToString("yyyyMMddHHmmss_") + 
                Path.GetFileNameWithoutExtension(modificationArg.FullPath);

            extactedPath = extactedPath + "\\" + bakBathDirName;

            Directory.CreateDirectory(extactedPath);
            Logger.Instance.WriteInfoLog("该批次文件备份放置于: " + extactedPath + " 目录已创建.");


            //调用第三方解压程序
            extactProgramArg = String.Format(extactProgramArg, extactedPath);

            Logger.Instance.WriteInfoLog("调用 " + extactProgram + " 参数: " + extactProgramArg);

            try
            {
                processReturnCode = UnZip(extactProgram, extactProgramArg);
                Logger.Instance.WriteInfoLog("调用过程结束, 返回值: " + processReturnCode);

                if (processReturnCode != 0)
                {
                    Logger.Instance.WriteInfoLog("处理过程中断.");
                    return OperationResult.Other;
                }

            }catch(Exception ex)
            {
                Logger.Instance.WriteInfoLog("调用第三方程序时出现错误: " + ex.Message + " 详细信息参见错误日志.");
                throw ex;
            }

            //递归备份
            Logger.Instance.WriteInfoLog("备份开始.");
            BakupRecursive(extactedPath, webSiteBasePath, bakupBasePath + "\\" + bakBathDirName);

            //递归覆盖
            Logger.Instance.WriteInfoLog("备份成功, 开始复制文件.");
            ConverRecursive(extactedPath, webSiteBasePath, bakupBasePath + "\\" + bakBathDirName);

            Logger.Instance.WriteInfoLog("自动处理结束.");
            return OperationResult.Success;
        }

        /// <summary>
        /// 递归覆盖
        /// </summary>
        /// <param name="path">参照目录</param>
        /// <param name="webSiteBasePath">操作目录</param>
        /// <param name="bakupTarget">目标目录</param>
        private void ConverRecursive(string path, string webSiteBasePath, string bakupTarget)
        {
            String[] filesNeedToConver = Directory.GetFiles(path);
            String[] dirsNeedToConver = Directory.GetDirectories(path);

            Logger.Instance.WriteInfoLog(String.Format("{0} 目录下需要处理 {1} 个文件, {2} 个目录.", path, filesNeedToConver.Length, dirsNeedToConver.Length));

            foreach (String file in filesNeedToConver)
            {
                String fileName = Path.GetFileName(file);

                Logger.Instance.WriteInfoLog("处理: " + fileName);

                if (!Directory.Exists(webSiteBasePath))
                {
                    Directory.CreateDirectory(webSiteBasePath);
                    Logger.Instance.WriteInfoLog("所在目录结构不存在, 已在操作目录创建: " + webSiteBasePath);
                }

                String source = path + "\\" + fileName;
                String dest = webSiteBasePath + "\\" + fileName;

                Logger.Instance.WriteInfoLog(String.Format("正在复制文件: {0} 到 {1}", source, dest));
                File.Copy(source, dest, true);
            }

            foreach (String dir in dirsNeedToConver)
            {
                String currentDirName = dir.Substring(dir.LastIndexOf("\\") + 1);

                Logger.Instance.WriteInfoLog("复制 - 处理下级目录: " + currentDirName);

                ConverRecursive(path + "\\" + currentDirName, webSiteBasePath + "\\" + currentDirName, bakupTarget + "\\" + currentDirName);
            }
        }


        /// <summary>
        /// 递归备份
        /// </summary>
        /// <param name="path">参照目录</param>
        /// <param name="webSiteBasePath">操作目录</param>
        /// <param name="bakupTarget">目标目录</param>
        private void BakupRecursive(string path, string webSiteBasePath, string bakupTarget)
        {
            //1 获取需要拷贝的文件和目录.
            //2 如果当前目录存在文件需要备份, 创建文件所在目录的整个结构, 并复制文件
            //  否则继续探测子级目录. 如果探测到叶级文件夹仍不包含任何文件, 该文件夹
            //  将不会被备份.
            //3 将子文件夹设置为参照目录, 继续向内探测
            
            String[] filesNeedToBakup = Directory.GetFiles(path);
            String[] dirsNeedToBakup = Directory.GetDirectories(path);

            Logger.Instance.WriteInfoLog(String.Format("{0} 目录下需要处理 {1} 个文件, {2} 个目录.", path, filesNeedToBakup.Length, dirsNeedToBakup.Length));

            foreach(String file in filesNeedToBakup)
            {
                String fileName = Path.GetFileName(file);

                Logger.Instance.WriteInfoLog("处理: " + fileName);

                if (!Directory.Exists(bakupTarget))
                {
                    Directory.CreateDirectory(bakupTarget);
                    Logger.Instance.WriteInfoLog("备份目标所在目录结构不存在, 已在备份目录创建: " + bakupTarget);
                }

                String source = webSiteBasePath + "\\" + fileName;
                String dest = bakupTarget + "\\" + fileName;

                Logger.Instance.WriteInfoLog(String.Format("正在复制文件: {0} 到 {1}", source, dest));

                if (File.Exists(webSiteBasePath + "\\" + fileName))
                    File.Copy(source, dest, true);
                else
                    Logger.Instance.WriteInfoLog(String.Format("备份源中的文件: {0} 不存在, 忽略备份.", source));
            }

            foreach (String dir in dirsNeedToBakup)
            {
                String currentDirName = dir.Substring(dir.LastIndexOf("\\") + 1);

                Logger.Instance.WriteInfoLog("备份 - 处理下级目录: " + currentDirName);

                BakupRecursive(path + "\\" + currentDirName, webSiteBasePath + "\\" + currentDirName, bakupTarget + "\\" + currentDirName);
            }
            
        }


        /// <summary>   
        /// 利用第三方程序解压缩   
        /// </summary>   
        /// <param name="cmd">解压缩程序参数</param>
        /// <param name="extactExe">解压缩程序全路径</param>
        /// <returns>true 或 false。解压缩成功返回 true，反之，false。</returns>   
        public int UnZip(String extactExe, String cmd)
        {
            ProcessStartInfo startinfo;
            Process process;

            startinfo = new ProcessStartInfo();
            startinfo.FileName = extactExe;
            startinfo.Arguments = cmd;
            startinfo.WindowStyle = ProcessWindowStyle.Hidden;
            process = new Process();
            process.StartInfo = startinfo;
            process.Start();
            process.WaitForExit();
            int exitCode = process.ExitCode;

            while (!process.HasExited) ;

            process.Close();

            return exitCode;
        }
    }
}
