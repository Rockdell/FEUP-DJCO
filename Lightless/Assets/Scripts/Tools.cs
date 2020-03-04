using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools {

    public static float CalculateVerticalVelocity(Vector3 initialPosition, Vector3 finalPosition, float horizontalVelocity) {
        return (horizontalVelocity * (finalPosition.y - initialPosition.y)) / (finalPosition.x - initialPosition.x) - (Physics2D.gravity.y * ((finalPosition.x - initialPosition.x) / (horizontalVelocity))) / 2f;
    }

    public static Vector2 CalculatePositionInTime(Vector3 initialPosition, float horizontalVelocity, float verticalVelocity, float time) {
        return new Vector2(initialPosition.x + horizontalVelocity * time, initialPosition.y + verticalVelocity * time + (Physics2D.gravity.y * time * time) / 2f);
    }

}
