#region Copyright © 2012 Paul Welter. All rights reserved.
/*
Copyright © 2012 Paul Welter. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright
   notice, this list of conditions and the following disclaimer in the
   documentation and/or other materials provided with the distribution.
3. The name of the author may not be used to endorse or promote products
   derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks.Git
{
    /// <summary>
    /// A task for git to get the current commit hash.
    /// </summary>
    public class GitVersion : GitClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitVersion"/> class.
        /// </summary>
        public GitVersion()
        {
            Command = "status";            
            Revision = "HEAD";                    
        }

        /// <summary>
        /// Gets or sets the revision to get the version from. Default is HEAD.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// Gets or sets regex to get Version from 
        /// </summary>
        public string VersionRegex { get; set; }

        /// <summary>
        /// Gets or sets git version.
        /// </summary>
        [Output]
        public string Version { get; set; }        

        /// <summary>
        /// Generates the arguments.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void GenerateArguments(CommandLineBuilder builder)
        {            
            base.GenerateArguments(builder);

            builder.AppendSwitch(Revision);
        }

        /// <summary>
        /// Parses a single line of text to identify any errors or warnings in canonical format.
        /// </summary>
        /// <param name="singleLine">A single line of text for the method to parse.</param>
        /// <param name="messageImportance">A value of <see cref="T:Microsoft.Build.Framework.MessageImportance"/> that indicates the importance level with which to log the message.</param>
        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            if (messageImportance == this.StandardErrorLoggingImportance)
            {
                base.LogEventsFromTextOutput(singleLine, messageImportance);
            }
            else if (!string.IsNullOrEmpty(this.VersionRegex))
            {
                Match match = Regex.Match(singleLine, this.VersionRegex, RegexOptions.IgnoreCase);
                this.Version = match.Groups[1].Value;
            }
            else
            {
                this.Version = singleLine.Trim();
            }
        }
    }
}
