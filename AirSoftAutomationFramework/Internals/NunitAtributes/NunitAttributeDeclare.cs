


// Ignore Spelling: Nunit

using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Commands;
using System;

namespace AirSoftAutomationFramework.Internals.NunitAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RetryMenage : PropertyAttribute, IWrapSetUpTearDown
    {
        private readonly int _tryCount;

        /// <summary>
        /// Construct a <see cref="MyRetryAttribute" />
        /// </summary>
        /// <param name="tryCount">The maximum number of times the test should be run if it fails</param>
        public RetryMenage(int tryCount) : base(tryCount)
        {
            _tryCount = tryCount;
        }

        #region IWrapSetUpTearDown Members

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command)
        {
            return new RetryCommand(command, _tryCount);
        }
    }
}
#endregion