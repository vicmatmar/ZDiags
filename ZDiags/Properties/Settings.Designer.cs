﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZDiags.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string COM_DUT {
            get {
                return ((string)(this["COM_DUT"]));
            }
            set {
                this["COM_DUT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string COM_BLE {
            get {
                return ((string)(this["COM_BLE"]));
            }
            set {
                this["COM_BLE"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("97207484672")]
        public long MAC_Block_Start {
            get {
                return ((long)(this["MAC_Block_Start"]));
            }
            set {
                this["MAC_Block_Start"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("97207582463")]
        public long MAC_Block_End {
            get {
                return ((long)(this["MAC_Block_End"]));
            }
            set {
                this["MAC_Block_End"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Logs")]
        public string Log_Folder {
            get {
                return ((string)(this["Log_Folder"]));
            }
            set {
                this["Log_Folder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4.5")]
        public double LED_Red_On_Val {
            get {
                return ((double)(this["LED_Red_On_Val"]));
            }
            set {
                this["LED_Red_On_Val"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3.8")]
        public double LED_Red_Off_Val {
            get {
                return ((double)(this["LED_Red_Off_Val"]));
            }
            set {
                this["LED_Red_Off_Val"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3.8")]
        public double LED_Yellow_On_Val {
            get {
                return ((double)(this["LED_Yellow_On_Val"]));
            }
            set {
                this["LED_Yellow_On_Val"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2.8")]
        public double LED_Yellow_Off_Val {
            get {
                return ((double)(this["LED_Yellow_Off_Val"]));
            }
            set {
                this["LED_Yellow_Off_Val"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2.7")]
        public double LED_Green_On_Val {
            get {
                return ((double)(this["LED_Green_On_Val"]));
            }
            set {
                this["LED_Green_On_Val"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.8")]
        public double LED_Green_Off_Val {
            get {
                return ((double)(this["LED_Green_Off_Val"]));
            }
            set {
                this["LED_Green_Off_Val"] = value;
            }
        }
    }
}
