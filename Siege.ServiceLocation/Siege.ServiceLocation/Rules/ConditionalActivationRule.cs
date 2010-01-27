/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Conditional;

namespace Siege.ServiceLocation.Rules
{
    public class ConditionalActivationRule<TBaseService, TContext> : IConditionalActivationRule
    {
        private readonly Predicate<object> evaluation;

        public ConditionalActivationRule(Func<TContext, bool> evaluation)
        {
            this.evaluation = x => (x is TContext) ? evaluation.Invoke((TContext)x) : false;
        }

        public IConditionalUseCase<TBaseService> Then<TImplementingType>() where TImplementingType : TBaseService
        {
            ConditionalGenericUseCase<TBaseService> useCase = new ConditionalGenericUseCase<TBaseService>();

            useCase.SetActivationRule(this);
            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public IInstanceUseCase Then(TBaseService implementation)
        {
            ConditionalInstanceUseCase<TBaseService> useCase = new ConditionalInstanceUseCase<TBaseService>();

            useCase.SetActivationRule(this);
            useCase.BindTo(implementation);

            return useCase;
        }

        public bool Evaluate(object context)
        {
            return evaluation.Invoke(context);
        }

        public Type GetBoundType()
        {
            return typeof (TBaseService);
        }

        public IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new ContextEvaluationStrategy();
        }
    }
}