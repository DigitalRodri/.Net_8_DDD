﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Domain.Resources.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The account with email: &apos;{0}&apos; already exists.
        /// </summary>
        public static string AccountAlreadyExists {
            get {
                return ResourceManager.GetString("AccountAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no registered account with email: &apos;{0}&apos;.
        /// </summary>
        public static string AccountDoesNotExist {
            get {
                return ResourceManager.GetString("AccountDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The input password was incorrect.
        /// </summary>
        public static string IncorrectPassword {
            get {
                return ResourceManager.GetString("IncorrectPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The request must contain an Authorization JWT Bearer Token.
        /// </summary>
        public static string NoAuthorization {
            get {
                return ResourceManager.GetString("NoAuthorization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter &apos;{0}&apos; is null or empty.
        /// </summary>
        public static string NullOrEmptyParameter {
            get {
                return ResourceManager.GetString("NullOrEmptyParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter &apos;{0}&apos; is null.
        /// </summary>
        public static string NullParameter {
            get {
                return ResourceManager.GetString("NullParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Title &apos;{0}&apos; must have 5 characters or less.
        /// </summary>
        public static string TitleLengthError {
            get {
                return ResourceManager.GetString("TitleLengthError", resourceCulture);
            }
        }
    }
}
