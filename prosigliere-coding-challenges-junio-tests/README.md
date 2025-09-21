# Blog API Tests

This project contains comprehensive unit and integration tests for the Blog API.

## Test Structure

### Unit Tests
- **Controllers/BlogPostsControllerTests.cs** - Tests for the BlogPostsController
- **DTOs/DtoValidationTests.cs** - Tests for DTO validation attributes

### Integration Tests
- **Integration/BlogApiIntegrationTests.cs** - End-to-end API tests

### Test Helpers
- **Helpers/TestDataHelper.cs** - Common test data creation utilities

## Test Coverage

### BlogPostsController Tests
- ✅ GET /api/posts - Retrieve all blog posts
- ✅ POST /api/posts - Create new blog post
- ✅ GET /api/posts/{id} - Get specific blog post with comments
- ✅ POST /api/posts/{id}/comments - Add comment to blog post
- ✅ Error handling and validation scenarios
- ✅ Edge cases and boundary conditions

### DTO Validation Tests
- ✅ CreateBlogPostDto validation rules
- ✅ CreateCommentDto validation rules
- ✅ String length validations
- ✅ Required field validations

### Integration Tests
- ✅ Full HTTP request/response cycle testing
- ✅ Database integration testing
- ✅ End-to-end workflow testing
- ✅ API contract validation

## Technologies Used

- **xUnit** - Testing framework
- **FluentAssertions** - Assertion library for more readable tests
- **Moq** - Mocking framework for unit tests
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing framework
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database for testing

## Running the Tests

### Run all tests
```bash
dotnet test
```

### Run with detailed output
```bash
dotnet test --verbosity normal
```

### Run with code coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run specific test category
```bash
# Run only unit tests
dotnet test --filter "FullyQualifiedName!~Integration"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~Integration"
```

## Test Scenarios Covered

### Happy Path Scenarios
- Creating blog posts with valid data
- Retrieving existing blog posts
- Adding comments to existing posts
- Listing posts with comment counts

### Error Scenarios
- Invalid input validation
- Non-existent resource requests
- Database operation failures
- Malformed request data

### Edge Cases
- Empty databases
- Maximum length inputs
- Whitespace handling
- Concurrent operations

## Best Practices Implemented

1. **Arrange-Act-Assert Pattern** - Clear test structure
2. **Descriptive Test Names** - Tests clearly describe what they're testing
3. **Independent Tests** - Each test can run in isolation
4. **Test Data Builders** - Reusable test data creation
5. **Comprehensive Coverage** - Both positive and negative test cases
6. **Integration Testing** - Full application stack testing
7. **Mocking** - Isolated unit testing with mocked dependencies

## Continuous Integration

These tests are designed to run in CI/CD pipelines and provide:
- Fast feedback on code changes
- Regression detection
- API contract validation
- Code quality assurance
