﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibEasySave.Res {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LibEasySave.Res.Resource1", typeof(Resource1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;_cryptInfo&quot;: {
        ///    &quot;_allowEtx&quot;: [
        ///      &quot;.txt&quot;
        ///    ],
        ///    &quot;cryptMode&quot;: &quot;XOR&quot;,
        ///    &quot;_key&quot;: &quot;681257479207131073&quot;
        ///  }
        ///}
        ///.
        /// </summary>
        internal static string AppConfig {
            get {
                return ResourceManager.GetString("AppConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;_activLang&quot;: 0,
        ///  &quot;_tralstedTexts&quot;: {
        ///    &quot;EN&quot;: {
        ///      &quot;_eLangCode_EN&quot;: &quot;EN : English&quot;,
        ///      &quot;_eLangCode_FR&quot;: &quot;FR : French&quot;,
        ///      &quot;_eSavingMode_FULL&quot;: &quot;FULL : copy all files and overwrite the same files&quot;,
        ///      &quot;_eSavingMode_DIFF&quot;: &quot;DIFF : copy only files who aren&apos;t present in destination folder&quot;,
        ///      &quot;_welcome&quot;: &quot;welcome on a ProSoft Software : EasySave&quot;,
        ///      &quot;_bye&quot;: &quot;Thank you for your trust&quot;,
        ///      &quot;_disableText&quot;: &quot;DISABLE&quot;,
        ///      &quot;_answerModelView&quot;: &quot;output&quot;,
        ///      &quot;_failMsg&quot;: &quot;F [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LangData {
            get {
                return ResourceManager.GetString("LangData", resourceCulture);
            }
        }
    }
}
