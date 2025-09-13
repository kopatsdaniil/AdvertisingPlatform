using System.Text;
using AdvertisingPlatforms.Services;

namespace AdvertisingPlatforms.Tests
{
    public class PlatformServiceTests
    {
        private PlatformService _service = null!;

        [SetUp]
        public void Setup()
        {
            _service = new PlatformService();
        }

        private async Task LoadExampleFile()
        {
            const string fileContent = """
                                       Яндекс.Директ:/ru
                                       Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
                                       Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
                                       Крутая реклама:/ru/svrd
                                       """;

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            await _service.UploadFile(stream);
        }

        [Test]
        public async Task GetPlatformsByRegion_RuMsk_ShouldReturnExpected()
        {
            // Arrange
            await LoadExampleFile();

            // Act
            var result = _service.GetPlatformsByRegion("/ru/msk");

            // Assert
            Assert.That(result, Does.Contain("Газета уральских москвичей"));
            Assert.That(result, Does.Contain("Яндекс.Директ"));
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetPlatformsByRegion_RuSvrd_ShouldReturnExpected()
        {
            // Arrange
            await LoadExampleFile();

            // Act
            var result = _service.GetPlatformsByRegion("/ru/svrd");

            // Assert
            Assert.That(result, Does.Contain("Яндекс.Директ"));
            Assert.That(result, Does.Contain("Крутая реклама"));
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetPlatformsByRegion_RuSvrdRevda_ShouldReturnExpected()
        {
            // Arrange
            await LoadExampleFile();

            // Act
            var result = _service.GetPlatformsByRegion("/ru/svrd/revda");

            // Assert
            Assert.That(result, Does.Contain("Яндекс.Директ"));
            Assert.That(result, Does.Contain("Ревдинский рабочий"));
            Assert.That(result, Does.Contain("Крутая реклама"));
            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task GetPlatformsByRegion_Ru_ShouldReturnExpected()
        {
            // Arrange
            await LoadExampleFile();

            // Act
            var result = _service.GetPlatformsByRegion("/ru");

            // Assert
            Assert.That(result, Does.Contain("Яндекс.Директ"));
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public void UploadFile_ShouldThrowException_WhenFileIsEmpty()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act + Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UploadFile(stream));
        }

        [Test]
        public async Task GetPlatformsByRegion_ShouldReturnEmptyList_WhenNoMatch()
        {
            // Arrange
            await LoadExampleFile();

            // Act
            var result = _service.GetPlatformsByRegion("/eu");

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}
