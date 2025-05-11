using System.Collections;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [System.Serializable]
    public class SlimeShape
    {
        public string shapeName;
        public Vector2[] pointPositions; // Stores local positions for each point
    }

    [Header("Shape Configuration")]
    [SerializeField] private SlimeShape defaultShape;
    [SerializeField] private SlimeShape triangleShape;
    [SerializeField] private SlimeShape squareShape;
    
    [Header("Duration Configuration")]
    [SerializeField] private float blueShapeDuration = 3f;  // Default shape duration (revert)
    [SerializeField] private float redShapeDuration = 5f;   // Triangle shape duration
    [SerializeField] private float greenShapeDuration = 7f; // Square shape duration

    [Header("Transition")]
    [SerializeField] private float transitionSpeed = 5f;    // Speed of shape transition
    [SerializeField] private bool useSmoothing = true;      // Use smooth transitions
    
    [Header("References")]
    [SerializeField] private SoftBody softBody;
    
    private Coroutine _currentShapeChangeCoroutine;
    private SlimeShape _currentShape;
    private Vector2[] _originalPointPositions; // Store the original positions of points

    private void Awake()
    {
        if (softBody == null)
        {
            softBody = GetComponent<SoftBody>();
            if (softBody == null)
            {
                Debug.LogError("SoftBody component not found. Please assign it in the inspector or add it to the same GameObject.");
            }
        }
        
        // Store original positions if the default shape isn't configured
        SaveOriginalPointPositions();
        
        // If the default shape isn't configured yet, set it from the current positions
        if (defaultShape == null || defaultShape.pointPositions == null || defaultShape.pointPositions.Length == 0)
        {
            SetupDefaultShape();
        }
        
        _currentShape = defaultShape;
    }

    private void SaveOriginalPointPositions()
    {
        if (softBody == null || softBody.points == null || softBody.points.Length == 0)
        {
            return;
        }

        _originalPointPositions = new Vector2[softBody.points.Length];
        for (int i = 0; i < softBody.points.Length; i++)
        {
            _originalPointPositions[i] = softBody.points[i].localPosition;
        }
    }

    private void SetupDefaultShape()
    {
        if (softBody == null || softBody.points == null || softBody.points.Length == 0)
        {
            return;
        }

        defaultShape = new SlimeShape
        {
            shapeName = "Default",
            pointPositions = new Vector2[softBody.points.Length]
        };

        for (int i = 0; i < softBody.points.Length; i++)
        {
            defaultShape.pointPositions[i] = softBody.points[i].localPosition;
        }
    }

    /// <summary>
    /// Changes the slime's shape based on the food type
    /// </summary>
    /// <param name="foodType">The type of food eaten</param>
    public void ChangeShape(System.Type foodType)
    {
        float duration = 0f;
        SlimeShape targetShape = null;

        // Determine which shape to use based on the food type
        if (foodType == typeof(RedFood))
        {
            targetShape = triangleShape;
            duration = redShapeDuration;
        }
        else if (foodType == typeof(GreenFood))
        {
            targetShape = squareShape;
            duration = greenShapeDuration;
        }
        else if (foodType == typeof(BlueFood))
        {
            targetShape = defaultShape;
            duration = blueShapeDuration;
        }
        else
        {
            Debug.LogWarning("Unknown food type: " + foodType.Name);
            return;
        }

        // Verify the shape has been configured
        if (targetShape == null || targetShape.pointPositions == null || targetShape.pointPositions.Length == 0)
        {
            Debug.LogWarning("Target shape not configured properly: " + foodType.Name);
            return;
        }

        // Cancel any ongoing shape change
        if (_currentShapeChangeCoroutine != null)
        {
            StopCoroutine(_currentShapeChangeCoroutine);
        }

        // Apply the new shape and start the timer to revert
        _currentShapeChangeCoroutine = StartCoroutine(ChangeShapeTemporarily(targetShape, duration));
    }

    /// <summary>
    /// Coroutine to temporarily change the shape and revert to the default shape after duration
    /// </summary>
    private IEnumerator ChangeShapeTemporarily(SlimeShape targetShape, float duration)
    {
        SlimeShape previousShape = _currentShape;
        _currentShape = targetShape;
        
        // Apply the new shape with smooth transition
        if (useSmoothing)
        {
            yield return StartCoroutine(SmoothlyTransitionToShape(targetShape));
        }
        else
        {
            ApplyShapeInstantly(targetShape);
        }
        
        
        
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);
        
        // If the shape hasn't been changed during the wait, revert to default
        if (_currentShape == targetShape)
        {
            _currentShape = defaultShape;
            
            // Smoothly transition back to default shape
            if (useSmoothing)
            {
                yield return StartCoroutine(SmoothlyTransitionToShape(defaultShape));
            }
            else
            {
                ApplyShapeInstantly(defaultShape);
            }
        }
        
        _currentShapeChangeCoroutine = null;
    }

    /// <summary>
    /// Instantly apply a shape by setting the points to their new positions
    /// </summary>
    private void ApplyShapeInstantly(SlimeShape shape)
    {
        if (softBody == null || softBody.points == null || shape == null || shape.pointPositions == null)
        {
            return;
        }

        // Ensure we have the right number of points
        int pointCount = Mathf.Min(softBody.points.Length, shape.pointPositions.Length);
        
        for (int i = 0; i < pointCount; i++)
        {
            softBody.points[i].localPosition = shape.pointPositions[i];
        }
    }

    /// <summary>
    /// Smoothly transitions to a new shape
    /// </summary>
    private IEnumerator SmoothlyTransitionToShape(SlimeShape targetShape)
    {
        if (softBody == null || softBody.points == null || targetShape == null || targetShape.pointPositions == null)
        {
            yield break;
        }

        // Calculate the start positions and target positions
        Vector2[] startPositions = new Vector2[softBody.points.Length];
        Vector2[] endPositions = new Vector2[softBody.points.Length];
        
        int pointCount = Mathf.Min(softBody.points.Length, targetShape.pointPositions.Length);
        
        for (int i = 0; i < pointCount; i++)
        {
            startPositions[i] = softBody.points[i].localPosition;
            endPositions[i] = targetShape.pointPositions[i];
        }
        
        // Lerp between the shapes
        float elapsedTime = 0f;
        float transitionDuration = 1f / transitionSpeed;
        
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            
            // Apply easing function for smoother transition
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            // Update positions
            for (int i = 0; i < pointCount; i++)
            {
                softBody.points[i].localPosition = Vector2.Lerp(startPositions[i], endPositions[i], smoothT);
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we reach the final positions
        for (int i = 0; i < pointCount; i++)
        {
            softBody.points[i].localPosition = endPositions[i];
        }
    }

    /// <summary>
    /// For editor use: record the current shape of the softbody
    /// </summary>
    public void CaptureCurrentShapeAsDefault()
    {
        if (softBody == null || softBody.points == null || softBody.points.Length == 0)
        {
            Debug.LogWarning("Cannot capture shape: SoftBody or its points are not properly set up");
            return;
        }

        SetupDefaultShape();
        Debug.Log("Default shape captured");
    }

    /// <summary>
    /// For editor use: create and save a triangle shape
    /// </summary>
    public void CreateTriangleShape()
    {
        if (softBody == null || softBody.points == null || softBody.points.Length == 0)
        {
            Debug.LogWarning("Cannot create shape: SoftBody or its points are not properly set up");
            return;
        }

        int pointCount = softBody.points.Length;
        triangleShape = new SlimeShape
        {
            shapeName = "Triangle",
            pointPositions = new Vector2[pointCount]
        };

        // Calculate a triangle shape based on the number of points
        float radius = 4.5f; // Adjust the size as needed
        Vector2 top = new Vector2(0, radius);
        Vector2 bottomLeft = new Vector2(-radius * 0.866f, -radius * 0.5f);
        Vector2 bottomRight = new Vector2(radius * 0.866f, -radius * 0.5f);

        for (int i = 0; i < pointCount; i++)
        {
            float t = (float)i / pointCount;
            
            if (t < 1.0f / 3.0f)
            {
                // Points between top and bottom right
                float segment = t * 3.0f;
                triangleShape.pointPositions[i] = Vector2.Lerp(top, bottomRight, segment);
            }
            else if (t < 2.0f / 3.0f)
            {
                // Points between bottom right and bottom left
                float segment = (t - 1.0f / 3.0f) * 3.0f;
                triangleShape.pointPositions[i] = Vector2.Lerp(bottomRight, bottomLeft, segment);
            }
            else
            {
                // Points between bottom left and top
                float segment = (t - 2.0f / 3.0f) * 3.0f;
                triangleShape.pointPositions[i] = Vector2.Lerp(bottomLeft, top, segment);
            }
        }

        Debug.Log("Triangle shape created");
    }

    /// <summary>
    /// For editor use: create and save a square shape
    /// </summary>
    public void CreateSquareShape()
    {
        if (softBody == null || softBody.points == null || softBody.points.Length == 0)
        {
            Debug.LogWarning("Cannot create shape: SoftBody or its points are not properly set up");
            return;
        }

        int pointCount = softBody.points.Length;
        squareShape = new SlimeShape
        {
            shapeName = "Square",
            pointPositions = new Vector2[pointCount]
        };

        // Calculate a square shape based on the number of points
        float halfSize = 2.5f; // Adjust the size as needed
        Vector2 topLeft = new Vector2(-halfSize, halfSize);
        Vector2 topRight = new Vector2(halfSize, halfSize);
        Vector2 bottomRight = new Vector2(halfSize, -halfSize);
        Vector2 bottomLeft = new Vector2(-halfSize, -halfSize);

        int pointsPerSide = pointCount / 4;
        int remainder = pointCount % 4;

        int currentPoint = 0;

        // Top side
        for (int i = 0; i < pointsPerSide + (remainder > 0 ? 1 : 0); i++)
        {
            float t = (float)i / (pointsPerSide + (remainder > 0 ? 1 : 0) - 1);
            squareShape.pointPositions[currentPoint++] = Vector2.Lerp(topLeft, topRight, t);
        }

        // Right side
        for (int i = 0; i < pointsPerSide + (remainder > 1 ? 1 : 0); i++)
        {
            float t = (float)i / (pointsPerSide + (remainder > 1 ? 1 : 0) - 1);
            squareShape.pointPositions[currentPoint++] = Vector2.Lerp(topRight, bottomRight, t);
        }

        // Bottom side
        for (int i = 0; i < pointsPerSide + (remainder > 2 ? 1 : 0); i++)
        {
            float t = (float)i / (pointsPerSide + (remainder > 2 ? 1 : 0) - 1);
            squareShape.pointPositions[currentPoint++] = Vector2.Lerp(bottomRight, bottomLeft, t);
        }

        // Left side
        for (int i = 0; i < pointsPerSide; i++)
        {
            float t = (float)i / (pointsPerSide - 1);
            squareShape.pointPositions[currentPoint++] = Vector2.Lerp(bottomLeft, topLeft, t);
        }

        Debug.Log("Square shape created");
    }

    /// <summary>
    /// Resets the shape to the default
    /// </summary>
    public void ResetToDefault()
    {
        if (_currentShapeChangeCoroutine != null)
        {
            StopCoroutine(_currentShapeChangeCoroutine);
            _currentShapeChangeCoroutine = null;
        }
        
        _currentShape = defaultShape;
        
        if (useSmoothing)
        {
            StartCoroutine(SmoothlyTransitionToShape(defaultShape));
        }
        else
        {
            ApplyShapeInstantly(defaultShape);
        }
    }
}