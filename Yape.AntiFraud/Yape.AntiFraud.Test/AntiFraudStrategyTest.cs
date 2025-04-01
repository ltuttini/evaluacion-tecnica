using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.AntiFraud.Strategy;

namespace Yape.AntiFraud.Test
{
    public class AntiFraudStrategyTest
    {
        [Fact]
        public void TransactionLimit_Validate_Amount()
        {
            var transactionLimit = new TransactionLimitStrategy(1000);

            Assert.True(transactionLimit.Limit(100));
            Assert.False(transactionLimit.Limit(2000));
        }

        [Fact]
        public void StrategyFactory_Amount_Ok()
        {
            var strategy1 = new Mock<IRejectionStrategy>();
            strategy1.Setup(x => x.Limit(It.IsAny<decimal>())).Returns(true);

            var strategy2 = new Mock<IRejectionStrategy>();
            strategy2.Setup(x => x.Limit(It.IsAny<decimal>())).Returns(true);

            var factory = new AntiFraudStrategyFactory();
            factory.AddStrategy(strategy1.Object);
            factory.AddStrategy(strategy2.Object);

            var result = factory.ApplyLimits(1000);

            Assert.True(result);
        }

        [Fact]
        public void StrategyFactory_Amount_Fail()
        {
            var strategy1 = new Mock<IRejectionStrategy>();
            strategy1.Setup(x => x.Limit(It.IsAny<decimal>())).Returns(false);

            var strategy2 = new Mock<IRejectionStrategy>();
            strategy2.Setup(x => x.Limit(It.IsAny<decimal>())).Returns(true);

            var factory = new AntiFraudStrategyFactory();
            factory.AddStrategy(strategy1.Object);
            factory.AddStrategy(strategy2.Object);

            var result = factory.ApplyLimits(1000);

            Assert.False(result);
        }

    }
}
