using FluentAssertions;
using Moq;
using SPublisher.Core;
using SPublisher.DBManagement;
using Xunit;

namespace SPublisher.UnitTests.DBManagement
{
    public class DatabaseCreatorTests
    {
        private const string DatabaseName = "DatabaseName";
        private readonly Mock<ISqlServerDataProvider> _sqlServerDataProviderMock =  new Mock<ISqlServerDataProvider>();
        private readonly Mock<ISqlServerDataProviderFactory> _sqlServerDataProviderFactoryMock = new Mock<ISqlServerDataProviderFactory>();
        private readonly Mock<IDatabase> _databaseCreateMock = new Mock<IDatabase>();
        private readonly IDatabaseCreator _databaseCreator;

        public DatabaseCreatorTests()
        {
            _databaseCreateMock.SetupGet(x => x.DatabaseName).Returns(DatabaseName);
            _sqlServerDataProviderFactoryMock.Setup(x => x.Get()).Returns(_sqlServerDataProviderMock.Object);
            _databaseCreator = new DatabaseCreator(_sqlServerDataProviderFactoryMock.Object);
        }

        [Fact]
        public void ShouldCreateDatabaseIfNotExists()
        {
            _sqlServerDataProviderMock.Setup(x=>x.DataBaseExists(DatabaseName)).Returns(false);
            var result = _databaseCreator.Create(_databaseCreateMock.Object);
            _sqlServerDataProviderMock.Verify(x=>x.CreateDataBase(_databaseCreateMock.Object), Times.Once);
            result.Should().Be(DatabaseCreateResult.Success);
        }

        [Fact]
        public void ShouldNotCreateDatabaseIfExists()
        {
            _sqlServerDataProviderMock.Setup(x => x.DataBaseExists(DatabaseName)).Returns(true);
            var result = _databaseCreator.Create(_databaseCreateMock.Object);
            _sqlServerDataProviderMock.Verify(x => x.CreateDataBase(_databaseCreateMock.Object), Times.Never);
            result.Should().Be(DatabaseCreateResult.AlreadyExists);
        }
    }
}