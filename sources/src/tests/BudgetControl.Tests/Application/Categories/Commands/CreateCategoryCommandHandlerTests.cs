using BudgetControl.Application.Categories.Commands;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Common.Primitives.Results;
using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Tests.Application.Categories.Commands;

[Collection("CategoryCommandHandlers"), Trait(nameof(CreateCategoryCommandHandler), "Unit"), ExcludeFromCodeCoverage]
public class CreateCategoryCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ILogger<CreateCategoryCommandHandler> _logger = Substitute.For<ILogger<CreateCategoryCommandHandler>>();
    private CreateCategoryCommandHandler _subject = null!;

    public CreateCategoryCommandHandlerTests()
    {
        _fixture.Customize<Category>(c => c.FromFactory(() => new CategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType("Credit")
            .Build()));
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateCategoryCommand(_fixture.Create<string>(), _fixture.Create<string>(), "Credit");
        _categoryRepository.AddAsync(Arg.Any<Category>(), CancellationToken.None).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new CreateCategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).AddAsync(Arg.Any<Category>(), CancellationToken.None);
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Description", "Credit")]
    [InlineData("Title", "", "Credit")]
    [InlineData("Title", "Description", "InvalidType")]
    public async Task Handle_InvalidCommand_ReturnsFailure(string title, string description, string type)
    {
        // Arrange
        var command = new CreateCategoryCommand(title, description, type);
        _categoryRepository.AddAsync(Arg.Any<Category>(), CancellationToken.None).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new CreateCategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.DidNotReceive().AddAsync(Arg.Any<Category>(), CancellationToken.None);
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_FailedToCommit_ReturnsFailure()
    {
        // Arrange
        var command = new CreateCategoryCommand(_fixture.Create<string>(), _fixture.Create<string>(), "Credit");
        _categoryRepository.AddAsync(Arg.Any<Category>(), CancellationToken.None).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Failures(_fixture.CreateMany<Error>()));

        _subject = new CreateCategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).AddAsync(Arg.Any<Category>(), CancellationToken.None);
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }
}
