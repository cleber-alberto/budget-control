using BudgetControl.Application.Categories.Commands;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Common.Primitives.Results;
using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Tests.Application.Categories.Commands;

[Collection("CategoryCommandHandlers"), Trait(nameof(DeleteCategoryCommandHandler), "Unit"), ExcludeFromCodeCoverage]
public class DeleteCategoryCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ILogger<DeleteCategoryCommandHandler> _logger = Substitute.For<ILogger<DeleteCategoryCommandHandler>>();
    private DeleteCategoryCommandHandler _subject = null!;

    public DeleteCategoryCommandHandlerTests()
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
        var command = new DeleteCategoryCommand(_fixture.Create<Guid>());
        _categoryRepository.GetByIdAsync(Arg.Any<CategoryId>(), Arg.Any<CancellationToken>()).Returns(_fixture.Create<Category>());
        _categoryRepository.Delete(Arg.Any<Category>());
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new DeleteCategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdAsync(Arg.Any<CategoryId>(), Arg.Any<CancellationToken>());
        _categoryRepository.Received(1).Delete(Arg.Any<Category>());
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteCategoryCommand(_fixture.Create<Guid>());
        _categoryRepository.GetByIdAsync(Arg.Any<CategoryId>(), Arg.Any<CancellationToken>()).Returns((Category?)null);
        _categoryRepository.Delete(Arg.Any<Category>());
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new DeleteCategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdAsync(Arg.Any<CategoryId>(), Arg.Any<CancellationToken>());
        _categoryRepository.DidNotReceive().Delete(Arg.Any<Category>());
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_FailedCommit_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteCategoryCommand(_fixture.Create<Guid>());
        _categoryRepository.GetByIdAsync(Arg.Any<CategoryId>(), Arg.Any<CancellationToken>()).Returns(_fixture.Create<Category>());
        _categoryRepository.Delete(Arg.Any<Category>());
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Failures(_fixture.CreateMany<Error>()));

        _subject = new DeleteCategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdAsync(Arg.Any<CategoryId>(), Arg.Any<CancellationToken>());
        _categoryRepository.Received(1).Delete(Arg.Any<Category>());
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }
}
