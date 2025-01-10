public class LevelObjectException : System.Exception {
    public LevelObjectErrorType ErrorType { get; }

    public LevelObjectException(LevelObjectErrorType errorType) : base($"Error: {errorType}") {
        ErrorType = errorType;
    }

    public LevelObjectException(LevelObjectErrorType errorType, string message) : base(message) {
        ErrorType = errorType;
    }
}

public enum LevelObjectErrorType {
    InvalidObjectsCreation,
    InvalidHexagonObjectType,
}