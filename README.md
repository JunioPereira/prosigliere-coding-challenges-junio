# Blog API - Backend Coding Challenge

A RESTful API for managing a simple blogging platform built with ASP.NET Core 8.0. This API provides endpoints for managing blog posts and their associated comments.

## Features

- ✅ Create and retrieve blog posts
- ✅ Add comments to blog posts
- ✅ View all posts with comment counts
- ✅ View individual posts with all comments
- ✅ Input validation and error handling
- ✅ Comprehensive API documentation with Swagger
- ✅ In-memory database with seed data
- ✅ Production-ready code structure
- ✅ Comprehensive unit and integration test suite

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: Entity Framework Core with In-Memory Database
- **Documentation**: Swagger/OpenAPI
- **Testing**: xUnit, FluentAssertions, Moq, ASP.NET Core Testing
- **Language**: C# 12

## API Endpoints

### Blog Posts

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/posts` | Get all blog posts with comment counts |
| POST | `/api/posts` | Create a new blog post |
| GET | `/api/posts/{id}` | Get a specific blog post with all comments |
| POST | `/api/posts/{id}/comments` | Add a comment to a specific blog post |

### Request/Response Examples

#### GET /api/posts
```json
[
  {
    "id": 1,
    "title": "Welcome to Our Blog",
    "commentCount": 1,
    "createdAt": "2024-01-15T10:00:00Z",
    "updatedAt": "2024-01-15T10:00:00Z"
  }
]
```

#### POST /api/posts
**Request:**
```json
{
  "title": "My New Blog Post",
  "content": "This is the content of my blog post..."
}
```

**Response:**
```json
{
  "id": 3,
  "title": "My New Blog Post",
  "content": "This is the content of my blog post...",
  "createdAt": "2024-01-20T15:30:00Z",
  "updatedAt": "2024-01-20T15:30:00Z",
  "comments": []
}
```

#### GET /api/posts/{id}
```json
{
  "id": 1,
  "title": "Welcome to Our Blog",
  "content": "This is the first blog post on our platform...",
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2024-01-15T10:00:00Z",
  "comments": [
    {
      "id": 1,
      "content": "Great introduction! Looking forward to more posts.",
      "author": "John Doe",
      "createdAt": "2024-01-16T09:00:00Z"
    }
  ]
}
```

#### POST /api/posts/{id}/comments
**Request:**
```json
{
  "content": "This is a great post!",
  "author": "Jane Smith"
}
```

**Response:**
```json
{
  "id": 4,
  "content": "This is a great post!",
  "author": "Jane Smith",
  "createdAt": "2024-01-20T16:00:00Z"
}
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Any IDE that supports .NET (Visual Studio, VS Code, Rider, etc.)

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd prosigliere-coding-challenges-junio
   ```

2. **Navigate to the project directory**
   ```bash
   cd prosigliere-coding-challenges-junio
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - API Base URL: `https://localhost:7xxx` or `http://localhost:5xxx`
   - Swagger Documentation: Navigate to the base URL in your browser
   - The application will automatically open Swagger UI showing all available endpoints

### Testing the API

#### Manual Testing

You can test the API using:

1. **Swagger UI** (recommended for development)
   - Navigate to the application URL in your browser
   - Interactive documentation with "Try it out" functionality

2. **HTTP Client** (Postman, Insomnia, etc.)
   - Import the endpoints from the Swagger JSON
   - Base URL: `https://localhost:7xxx/api`

3. **cURL Examples**
   ```bash
   # Get all posts
   curl -X GET "https://localhost:7xxx/api/posts"
   
   # Create a new post
   curl -X POST "https://localhost:7xxx/api/posts" \
        -H "Content-Type: application/json" \
        -d '{"title":"Test Post","content":"This is a test post"}'
   
   # Get a specific post
   curl -X GET "https://localhost:7xxx/api/posts/1"
   
   # Add a comment
   curl -X POST "https://localhost:7xxx/api/posts/1/comments" \
        -H "Content-Type: application/json" \
        -d '{"content":"Great post!","author":"John Doe"}'
   ```

#### Automated Testing

The project includes a comprehensive test suite with **31 tests (100% passing)**:

```bash
# Navigate to test project
cd prosigliere-coding-challenges-junio-tests

# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test categories
dotnet test --filter "BlogPostsController"  # Controller tests
dotnet test --filter "DtoValidation"        # Validation tests
dotnet test --filter "SimpleIntegration"    # Integration tests
```

**Test Coverage:**
- ✅ **9 Controller Tests** - All API endpoints and error scenarios
- ✅ **17 DTO Validation Tests** - Input validation and constraints
- ✅ **5 Integration Tests** - End-to-end API functionality

For detailed test documentation, see `prosigliere-coding-challenges-junio-tests/README.md`

## Project Structure

```
prosigliere-coding-challenges-junio/
├── Controllers/
│   └── BlogPostsController.cs      # Main API controller
├── Data/
│   └── BlogContext.cs              # Entity Framework DbContext
├── DTOs/
│   ├── BlogPostDto.cs              # Blog post data transfer objects
│   └── CommentDto.cs               # Comment data transfer objects
├── Models/
│   ├── BlogPost.cs                 # Blog post entity
│   └── Comment.cs                  # Comment entity
├── Program.cs                      # Application entry point
├── README.md                       # This file
└── prosigliere-coding-challenges-junio-tests/  # Test project
    ├── Controllers/                # Controller unit tests
    ├── DTOs/                       # DTO validation tests
    ├── Integration/                # Integration tests
    ├── Helpers/                    # Test utilities
    └── README.md                   # Test documentation
```

## Data Models

### BlogPost
- `Id` (int): Unique identifier
- `Title` (string): Post title (1-200 characters)
- `Content` (string): Post content (1-5000 characters)
- `CreatedAt` (DateTime): Creation timestamp
- `UpdatedAt` (DateTime): Last update timestamp
- `Comments` (List<Comment>): Associated comments

### Comment
- `Id` (int): Unique identifier
- `Content` (string): Comment content (1-1000 characters)
- `Author` (string): Comment author name (1-100 characters)
- `CreatedAt` (DateTime): Creation timestamp
- `BlogPostId` (int): Foreign key to blog post

## Validation

The API includes comprehensive input validation:

- **Blog Posts**: Title and content are required with length constraints
- **Comments**: Content and author are required with length constraints
- **IDs**: Validated for existence before operations
- **Error Responses**: Detailed error messages for validation failures

## Error Handling

The API provides consistent error responses:

- `400 Bad Request`: Invalid input data
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Unexpected server errors

All errors include descriptive messages to help with debugging.

## Next Steps (If More Time Available)

### High Priority
- [ ] **Persistent Database**: Replace in-memory database with SQL Server/PostgreSQL
- [ ] **Authentication & Authorization**: Add JWT-based user authentication
- [ ] **Pagination**: Implement pagination for blog posts and comments
- [x] **Unit Tests**: Comprehensive test coverage for all endpoints ✅
- [x] **Integration Tests**: End-to-end API testing ✅

### Medium Priority
- [ ] **Blog Post Updates**: Add PUT/PATCH endpoints for updating posts
- [ ] **Comment Management**: Add endpoints to update/delete comments
- [ ] **Search Functionality**: Add search endpoints for posts by title/content
- [ ] **Rate Limiting**: Implement API rate limiting
- [ ] **Caching**: Add response caching for better performance

### Low Priority
- [ ] **File Uploads**: Support for images in blog posts
- [ ] **Categories/Tags**: Add categorization system
- [ ] **Email Notifications**: Notify authors of new comments
- [ ] **Admin Panel**: Web interface for content management
- [ ] **Analytics**: Track post views and engagement metrics

## Performance Considerations

- Uses Entity Framework Core with optimized queries
- Includes proper indexing on foreign keys
- Implements async/await pattern for non-blocking operations
- CORS enabled for cross-origin requests

## Security Features

- Input validation and sanitization
- SQL injection protection through EF Core
- HTTPS enforcement in production
- CORS policy configuration

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request
