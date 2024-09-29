using System;
using BudgetControl.Application.Categories.Commands;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Common.Primitives.Results;
using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Tests.Application.Categories.Commands;

[Collection("CategoryCommandHandlers"), Trait(nameof(CreateSubcategoryCommandHandler), "Unit"), ExcludeFromCodeCoverage]
public class CreateSubcategoryCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ILogger<CreateSubcategoryCommandHandler> _logger = Substitute.For<ILogger<CreateSubcategoryCommandHandler>>();
    private CreateSubcategoryCommandHandler _subject = null!;

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(_fixture.Create<Category>()));
        _categoryRepository.AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new CreateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        await _categoryRepository.Received(1).AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None);
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Description")]
    [InlineData("Title", "")]
    public async Task Handle_InvalidCommand_ReturnsFailure(string title, string description)
    {
        // Arrange
        var command = new CreateSubcategoryCommand(title, description);
        _categoryRepository.AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None).Returns(Task.CompletedTask);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(_fixture.Create<Category>()));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new CreateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        await _categoryRepository.DidNotReceive().AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None);
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_InvalidCategory_ReturnsFailure()
    {
        // Arrange
        var command = new CreateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult((Category?)null));
        _categoryRepository.AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new CreateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        await _categoryRepository.DidNotReceive().AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None);
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_FailedCommit_ReturnsFailure()
    {
        // Arrange
        var command = new CreateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(_fixture.Create<Category>()));
        _categoryRepository.AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Failures(_fixture.CreateMany<Error>()));

        _subject = new CreateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        await _categoryRepository.Received(1).AddSubcategoryAsync(Arg.Any<Subcategory>(), CancellationToken.None);
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }
}
