using Moq;
using NRules.Diagnostics;
using NRules.Rete;
using NUnit.Framework;

namespace NRules.Tests
{
    [TestFixture]
    public class SessionTest
    {
        private Mock<IAgenda> _agenda;
        private Mock<INetwork> _network;
        private Mock<IWorkingMemory> _workingMemory;
        private Mock<IEventAggregator> _eventAggregator;
        private Mock<IDependencyResolver> _dependencyResolver;
            
        [SetUp]
        public void Setup()
        {
            _agenda = new Mock<IAgenda>();
            _network = new Mock<INetwork>();
            _workingMemory = new Mock<IWorkingMemory>();
            _eventAggregator = new Mock<IEventAggregator>();
            _dependencyResolver = new Mock<IDependencyResolver>();
        }

        [Test]
        public void Insert_Called_PropagatesAssert()
        {
            // Arrange
            var fact = new object();
            var target = CreateTarget();
            _network.Setup(x => x.PropagateAssert(It.IsAny<IExecutionContext>(), fact)).Returns(true);

            // Act
            target.Insert(fact);

            // Assert
            _network.Verify(x => x.PropagateAssert(It.Is<IExecutionContext>(p => p.WorkingMemory == _workingMemory.Object), fact), Times.Exactly(1));
        }

        [Test]
        public void Update_Called_PropagatesUpdate()
        {
            // Arrange
            var fact = new object();
            var target = CreateTarget();
            _network.Setup(x => x.PropagateUpdate(It.IsAny<IExecutionContext>(), fact)).Returns(true);

            // Act
            target.Update(fact);

            // Assert
            _network.Verify(x => x.PropagateUpdate(It.Is<IExecutionContext>(p => p.WorkingMemory == _workingMemory.Object), fact), Times.Exactly(1));
        }

        [Test]
        public void Retract_Called_PropagatesRetract()
        {
            // Arrange
            var fact = new object();
            var target = CreateTarget();
            _network.Setup(x => x.PropagateRetract(It.IsAny<IExecutionContext>(), fact)).Returns(true);

            // Act
            target.Retract(fact);

            // Assert
            _network.Verify(x => x.PropagateRetract(It.Is<IExecutionContext>(p => p.WorkingMemory == _workingMemory.Object), fact), Times.Exactly(1));
        }

        private Session CreateTarget()
        {
            return new Session(_network.Object, _agenda.Object, _workingMemory.Object, _eventAggregator.Object, _dependencyResolver.Object);
        }
    }
}