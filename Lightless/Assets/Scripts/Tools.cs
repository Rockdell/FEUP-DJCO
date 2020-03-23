using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools {

    //public static float CalculateVerticalVelocity(Vector3 initialPosition, Vector3 finalPosition, float horizontalVelocity) {
    //    return (horizontalVelocity * (finalPosition.y - initialPosition.y)) / (finalPosition.x - initialPosition.x) - (Physics2D.gravity.y * ((finalPosition.x - initialPosition.x) / (horizontalVelocity))) / 2f;
    //}

    //public static Vector2 CalculatePositionInTime(Vector3 initialPosition, float horizontalVelocity, float verticalVelocity, float time) {
    //    return new Vector2(initialPosition.x + horizontalVelocity * time, initialPosition.y + verticalVelocity * time + (Physics2D.gravity.y * time * time) / 2f);
    //}

    public static Vector2 CalculateVelocity(Vector3 initialPosition, Vector3 finalPosition, float time) {
        return new Vector2((finalPosition.x - initialPosition.x) / time, (finalPosition.y - initialPosition.y) / time - (Physics2D.gravity.y * time) / 2f);
    }

    public static Vector2 CalculatePositionInTime(Vector3 initialPosition, Vector2 velocity, float time) {
        return new Vector2(initialPosition.x + velocity.x * time, initialPosition.y + velocity.y * time + (Physics2D.gravity.y * time * time) / 2f);
    }

    //public static bool IsPositionInScreen(Vector2 begin, Vector2 end)
    //{
    //    return
    //        begin.x >= -GameManager.Instance.screenBounds.x && posibegintion.x <= GameManager.Instance.screenBounds.x
    //        &&
    //        position.y >= -GameManager.Instance.screenBounds.y && position.y <= GameManager.Instance.screenBounds.y;
    //}

}
