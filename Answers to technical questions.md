# Answers to Technical Questions

## 1. How much time did you spend on this task?

I spent approximately 2-3 hours on this project, with a focused approach on understanding every step rather than just "vibe-coding":

- **Setting up the .NET Core project** - Learned about project structure and dependencies
- **OpenWeatherMap API integration** - Studied the API documentation to understand the {city} query format and response structure
- **Service layer implementation** - Used AI assistance to help with implementation patterns, but carefully reviewed and understood each piece of code to ensure proper error handling and architecture
- **Unit tests** - Learned about mocking with Moq and xUnit testing patterns
- **Documentation** - Ensured all documentation was clear and accurate

**My approach**: I used AI enhancements as a learning tool throughout the process, asking for explanations and checking every step to understand *why* code works the way it does, not just *what* it does. This ensures I can maintain, debug, and extend the code independently. The key for me is understanding every step deeply rather than blindly copying solutions.

## 2. If you had more time, what improvements or additions would you make?

If I had more time, I would add the following improvements:

### Backend Improvements:
1. **Caching**: Implement caching (Redis or in-memory) to reduce API calls to OpenWeatherMap and improve response times
2. **Rate Limiting**: Add rate limiting to prevent API abuse
3. **Logging**: Implement structured logging (Serilog) for better observability
4. **Retry Policy**: Add Polly for resilient HTTP calls with retry logic
5. **Integration Tests**: Test the full API flow
6. **More UI side help**: Would add a list of most common cities for front-end to improve potential typing mistakes and auto-fill

### Documentation:
1. **API Documentation**: Enhanced Swagger/OpenAPI documentation with examples
2. **README**: More detailed setup and deployment instructions
3. **Architecture Diagram**: Visual representation of the system

## 3. What is the most useful feature recently added to your favorite programming language?

As a Developer who just learned about Go(lang), one of the most useful recent features is **Generics** introduced in Go 1.18. This feature allows writing reusable code that works with different types without sacrificing type safety or requiring runtime type assertions.

### Example Usage:

A common scenario I encounter is working with slices of different types. Before generics, I had to write separate functions for each type:

**Before (Go 1.17 and earlier - duplicate functions for each type):**
```go
// Need separate functions for each type
func GetFirstInt(items []int) int {
    if len(items) == 0 {
        return 0
    }
    return items[0]
}

func GetFirstFloat(items []float64) float64 {
    if len(items) == 0 {
        return 0.0
    }
    return items[0]
}

func GetFirstString(items []string) string {
    if len(items) == 0 {
        return ""
    }
    return items[0]
}

// Usage:
temperatures := []float64{25.5, 28.3, 22.1}
firstTemp := GetFirstFloat(temperatures)

cities := []string{"Tehran", "London", "New York"}
firstCity := GetFirstString(cities)
```

**After (Go 1.18+ with Generics - one function for all types):**
```go
// One function works with any type!
func GetFirst[T any](items []T) (T, bool) {
    if len(items) == 0 {
        var zero T  // zero value for type T
        return zero, false
    }
    return items[0], true
}

// Usage with different types:
temperatures := []float64{25.5, 28.3, 22.1}
firstTemp, ok := GetFirst(temperatures)  // Works!

cities := []string{"Tehran", "London", "New York"}
firstCity, ok := GetFirst(cities)  // Same function!

humidityValues := []int{60, 75, 45}
firstHumidity, ok := GetFirst(humidityValues)  // Works with ints too!
```

This feature eliminates code duplication and allows me to write one function that works with any type, making my code cleaner and easier to maintain. It's especially helpful when creating utility functions, data structures, and API handlers that need to work with different types.

## 4. How do you identify and diagnose a performance issue in a production environment?

### Identification and Diagnosis Process:

1. **Monitoring and Alerting**:
   - Set up Application Performance Monitoring (APM) tools like Application Insights, New Relic, or Datadog
   - Configure alerts for response time thresholds, error rates, and resource utilization
   - Monitor key metrics: CPU, memory, disk I/O, network latency, database query times

2. **Log Analysis**:
   - Review structured logs for slow operations, exceptions, and warnings
   - Use log aggregation tools (ELK Stack, Splunk, Seq) to identify patterns
   - Look for spikes in error rates or unusual request patterns

3. **Performance Profiling**:
   - Use profiling tools (dotMemory, PerfView, Visual Studio Diagnostic Tools)
   - Identify CPU hotspots, memory leaks, and excessive allocations
   - Profile database queries and external API calls

4. **Distributed Tracing**:
   - Implement distributed tracing (OpenTelemetry, Jaeger, Zipkin)
   - Trace requests across services to identify bottlenecks
   - Analyze end-to-end request latency

5. **Database Performance**:
   - Monitor slow query logs
   - Analyze query execution plans
   - Check for missing indexes, table scans, and lock contention

6. **Code Review and Analysis**:
   - Review recent deployments for performance-related changes
   - Use static analysis tools to identify potential performance issues
   - Check for N+1 queries, inefficient algorithms, or blocking operations

### Have I done this before?

While I haven't worked in production environments yet (as I'm seeking an internship), I've learned about performance debugging through:

- **Online courses and tutorials** covering APM tools like Application Insights
- **Practice projects** where I've used profiling tools and performance counters to optimize my code
- **Reading technical articles** about common performance issues like N+1 queries, memory leaks, and database optimization
- **Security certifications (CEH, OWASP)** which have given me insights into monitoring and analyzing system behavior

In my personal projects, I've practiced identifying bottlenecks by:
- Using browser DevTools to analyze frontend performance
- Profiling Python scripts to find slow operations
- Optimizing database queries in PostgreSQL projects

I'm eager to apply this knowledge in a real production environment and learn from experienced developers about how these tools are used in practice.

## 5. What's the last technical book you read or technical conference you attended?

### Book: "Clean Architecture" by Robert C. Martin

**What I learned:**
- The importance of **separation of concerns** and **dependency inversion**
- How to structure applications using the **dependency rule** - dependencies should point inward toward business logic
- The concept of **use cases** as the core of the application, independent of frameworks and databases
- How to make systems **testable** and **maintainable** by keeping business logic isolated from infrastructure concerns
- The value of **independent deployability** and **framework independence**

While reading this book, I learned about important architectural concepts. In this Weather API project, I tried to apply basic principles like keeping controllers, services, and models separated - though I acknowledge this is a simple implementation and there's much more to learn about Clean Architecture in real-world applications. I'm looking forward to gaining practical experience with these concepts through an internship.

## 6. What's your opinion about this technical test?

I found this technical test to be **well-designed and practical**:

**Strengths:**
- The requirements are clear and achievable within a reasonable time frame
- It tests real-world skills: API integration, data transformation, and UI development
- The acceptance criteria are specific (Tehran example) which helps validate the solution
- It encourages clean code practices and testing, which are essential for production software
- The technical questions are thoughtful and allow candidates to demonstrate their experience and thought process

**Suggestions:**
- Perhaps include a requirement for error handling scenarios (invalid city, API failures)
- Could add a note about whether coordinates API endpoint is required or optional
- Maybe clarify if UI is mandatory or if API-only is acceptable

Overall, it's a good assessment that tests both coding ability and architectural thinking. The combination of coding exercise and technical questions provides a well-rounded view of a candidate's skills.

## 7. Please describe yourself using JSON format.

```json
{
  "name": "Parsa Mohammadi",
  "role": "Backend Developer",
  "location": {
    "city": "Tehran",
    "country": "Iran"
  },
  "experience": {
    "years": 0,
    "status": "Seeking internship opportunity",
    "background": "Architecture degree with strong problem-solving and algorithmic thinking skills"
  },
  "education": {
    "degree": "Bachelor of Architecture",
    "university": "Islamic Azad University, South Tehran Branch",
    "period": "2019-2025",
    "gpa": 16.33
  },
  "primaryTechnologies": {
    "advanced": [
      "Python",
      "Linux"
    ],
    "intermediate": [
      "Docker",
      "PostgreSQL",
      "Go (Golang)",
      ".NET Core / C#",
      "ASP.NET Core"
    ],
    "beginner": [
      "Git",
      "SQL Server",
      "MySQL"
    ]
  },
  "skills": {
    "backend": [
      "RESTful API Development",
      "Web Scraping",
      "Automation Tools",
      "Database Design",
      "Unit Testing"
    ],
    "security": [
      "Web Security (OWASP)",
      "Network Security (CEH)",
      "Security Best Practices"
    ],
    "tools": [
      "Docker",
      "PostgreSQL",
      "Linux Administration",
      "Git"
    ]
  },
  "certifications": [
    "LPIC 201",
    "Network+ / Security+",
    "CEH 2024",
    "OWASP Zero 2025",
    "Advanced Web Scraping",
    "Prompt Engineering 2023"
  ],
  "languages": {
    "english": "Native",
    "german": "Pre-Intermediate"
  },
  "traits": [
    "Motivated",
    "Problem Solver",
    "Algorithmic Thinker",
    "Continuous Learner",
    "Detail-Oriented"
  ],
  "interests": [
    "Fintech",
    "Web Security",
    "Network Security",
    "Backend Development",
    "System Design"
  ],
  "approach": {
    "coding": "Write clean, maintainable, and secure code following best practices",
    "learning": "Passionate about learning new technologies and applying security principles",
    "goal": "Seeking internship opportunity to apply technical skills in a real-world, challenging fintech environment"
  },
  "availability": "Available for internship positions in Tehran"
}
```

