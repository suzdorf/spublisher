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
        private readonly Mock<IDatabaseCreate> _databaseCreateMock = new Mock<IDatabaseCreate>();
        private readonly IDatabaseCreator _databaseCreator;

        public DatabaseCreatorTests()
        {
            _databaseCreateMock.SetupGet(x => x.DbName).Returns(DatabaseName);
            _databaseCreator = new DatabaseCreator(_sqlServerDataProviderMock.Object);
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