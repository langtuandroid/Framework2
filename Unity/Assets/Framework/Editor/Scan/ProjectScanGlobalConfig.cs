﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Framework.Editor
{
    public class ProjectScanGlobalConfig
    {
        public List<string> IncludeDir = new() { "Assets" };
        public List<string> IgnoreDir = new() { "Assets/Ignore" };
        public Dictionary<string, ScanRuleConfig> RuleConfig = new();
        public Dictionary<string, bool> MenuEnable = new();
        [JsonIgnore] public Dictionary<string, ScanRuleNameConfig> RuleNameConfig { get; } = new();
        [JsonIgnore] public Dictionary<string, List<string>> WhiteListDic = new();
        public ProjectScanGlobalConfig()
        {
            string ruleContent = File.ReadAllText(ProjectScanPath.LocalScanRuleTxtPath);
            var result = SerializeHelper.Deserialize<List<ScanRuleNameConfig>>(ruleContent);
            foreach (var rule in result)
            {
                RuleNameConfig[rule.Id] = rule;
            }

            if (File.Exists(ProjectScanPath.FixWhiteListPath))
            {
                string whiteList = File.ReadAllText(ProjectScanPath.FixWhiteListPath);
                WhiteListDic = SerializeHelper.Deserialize<Dictionary<string, List<string>>>(whiteList);
            }
        }

        public void Save()
        {
            File.WriteAllText(ProjectScanPath.ProjectScanConfigPath, this.ToNTJson());
            File.WriteAllText(ProjectScanPath.FixWhiteListPath, WhiteListDic.ToNTJson());
        }
    }

    /// <summary>
    /// 用来记录scan rule的描述 帮助 limit值的配置
    /// 实在不知道起什么名字了
    /// </summary>
    [Serializable]
    public struct ScanRuleNameConfig
    {
        public static ScanRuleNameConfig Empty = new() { Id = "-1" };
        public string Id;
        public string Name;
        public string Desc;
        public string HelpUrl;
    }

    /// <summary>
    /// 用来记录scan rule的自身配置
    /// </summary>
    public class ScanRuleConfig
    {
        public string Id;
        public bool IsEnable;
        public bool UseSelfDirConfig;
        public List<string> IncludeDir;
        public List<string> IgnoreDir;
        public string Value = string.Empty;

        public ScanRuleConfig()
        {
        }
        
        public ScanRuleConfig(ScanRule scanRule)
        {
            Id = scanRule.RuleId;
            IsEnable = scanRule.IsEnable;
            Value = scanRule.Value;
            if (scanRule is ScanRuleWithDir scanRuleWithDir)
            {
                UseSelfDirConfig = scanRuleWithDir.UseSelfDirConfig;
                IncludeDir = scanRuleWithDir.IncludeDir;
                IgnoreDir = scanRuleWithDir.IgnoreDir;
            }
            else
            {
                UseSelfDirConfig = false;
            }
        }
    }
}