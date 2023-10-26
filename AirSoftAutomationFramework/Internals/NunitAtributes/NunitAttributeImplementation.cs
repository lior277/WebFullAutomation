// Ignore Spelling: Nunit

using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using System;

namespace AirSoftAutomationFramework.Internals.NunitAttributes
{
    public class RetryCommand : DelegatingTestCommand
    {
        private readonly int _tryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryCommand"/> class.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        /// <param name="tryCount">The maximum number of repetitions</param>
        public RetryCommand(TestCommand innerCommand, int tryCount)
            : base(innerCommand)
        {
            _tryCount = tryCount;
        }

        /// <summary>
        /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
        /// </summary>
        /// <param name="context">The context in which the test should run.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            var count = _tryCount;

            try
            {
                context.CurrentResult = innerCommand.Execute(context);
            }
            // Commands are supposed to catch exceptions, but some don't
            // and we want to look at restructuring the API in the future.
            catch (Exception ex)
            {
                context.CurrentResult ??= context.CurrentTest.MakeTestResult();
                context.CurrentResult.RecordException(ex);
            }

            while (count-- > 0)
            {
                if (context.CurrentResult.ResultState != ResultState.Error
                    && context.CurrentResult.ResultState != ResultState.Failure
                    && context.CurrentResult.ResultState != ResultState.SetUpError
                    && context.CurrentResult.ResultState != ResultState.SetUpFailure
                    && context.CurrentResult.ResultState != ResultState.TearDownError
                    && context.CurrentResult.ResultState != ResultState.ChildFailure
                    && context.CurrentResult.ResultState != ResultState.Inconclusive)
                {
                    break;
                }
                else
                {
                    try
                    {
                        // Clear result for retry
                        if (count > 0)
                        {
                            var testMessage = TestContext.CurrentContext.Result.Message ?? " ";
                            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
                            context.CurrentResult = context.CurrentTest.MakeTestResult();
                            context.CurrentRepeatCount++; // increment Retry count for next iteration. will only happen if we are guaranteed another iteration              
                            context.OutWriter.WriteLine();

                            context.OutWriter.WriteLine($"Test: {context.CurrentTest.Name}," +
                            $" retried: {count} time/s.");

                            context.CurrentResult = innerCommand.Execute(context);
                        }
                    }
                    // Commands are supposed to catch exceptions, but some don't
                    // and we want to look at restructuring the API in the future.
                    catch (Exception ex)
                    {
                        context.CurrentResult ??= context.CurrentTest.MakeTestResult();
                        context.CurrentResult.RecordException(ex);
                    }
                }
            }

            return context.CurrentResult;
        }
    }
}
