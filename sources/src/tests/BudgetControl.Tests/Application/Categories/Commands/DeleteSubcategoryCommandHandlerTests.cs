using System;
using BudgetControl.Application.Categories.Commands;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Common.Primitives.Results;
using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Tests.Application.Categories.Commands;

[Collection("CategoryCommandHandlers"), Trait(nameof(DeleteSubcategoryCommandHandler), "Unit"), ExcludeFromCodeCoverage]
public class DeleteSubcategoryCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ILogger<DeleteSubcategoryCommandHandler> _logger = Substitute.For<ILogger<DeleteSubcategoryCommandHandler>>();
    private DeleteSubcategoryCommandHandler _subject = null!;

    public DeleteSubcategoryCommandHandlerTests()
    {
        _fixture.Customize<Category>(c => c.FromFactory(() => new CategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType("Credit")
            .WithSubcategories(new SubcategoryBuilder()
                .WithTitle(_fixture.Create<string>())
                .WithDescription(_fixture.Create<string>())
                .Build())
            .Build()));
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var command = new DeleteSubcategoryCommand(category.Subcategories.First().Id.Value);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(category));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new DeleteSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.Received(1).DeleteSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteSubcategoryCommand(Guid.Empty);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(_fixture.Create<Category>()));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new DeleteSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        await _unitOfWork.Received(0).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteSubcategoryCommand(Guid.Empty);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult<Category?>(null));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new DeleteSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        await _unitOfWork.Received(0).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_FailedToCommit_ReturnsFailure()
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var command = new DeleteSubcategoryCommand(category.Subcategories.First().Id.Value);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(category));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Failures(_fixture.CreateMany<Error>()));

        _subject = new DeleteSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.Received(1).DeleteSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }
}
