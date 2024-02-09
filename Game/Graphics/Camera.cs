// Ignore Spelling: Calc

using Microsoft.Xna.Framework;

using System;
using System.Diagnostics;

namespace GameJam14.Game.Graphics;
internal class Camera {
    public static readonly float maxZoom = 2000.0f;
    public static readonly float minZoom = 0.1f;
    public Camera(Screen screen) {
        this.aspectRatio = screen.Width / (float)screen.Height;
        this.fieldOfView = (float)( Math.PI / 2.0f );

        this.position = Vector2.Zero;
        this._baseZoom = this.CalcZoomFromHeight(screen.Height);
        this._zoom = this._baseZoom;
        this._incrementZoom = 10f;

        this.UpdateViewMatrix();
        this.UpdateProjectionMatrix();
    }

    public Vector2 Position { get { return this.position; } }
    public Matrix Projection { get { return this.projection; } }
    public Matrix View { get { return this.view; } }
    public float Zoom { get { return this._baseZoom; } }
    public float CalcZoomFromHeight(float height) {
        Debug.WriteLine(
            "Height: " + height + "\n" +
            "Field of View: " + this.fieldOfView + "\n" +
            "Tan: " + MathF.Tan(this.fieldOfView / 2.0f) + "\n" +
            "Zoom: " + ( height / ( 2.0f * MathF.Tan(this.fieldOfView / 2.0f) ) )
        );
        return height / ( 2.0f * MathF.Tan(this.fieldOfView / 2.0f) );
    }

    public void GetExtents(out Vector2 topLeft, out Vector2 bottomRight, out Vector2 center) {
        float tanTheta = (float)Math.Tan(this.fieldOfView / 2.0f);
        float halfHeight = this._zoom * tanTheta;
        float halfWidth = halfHeight * this.aspectRatio;

        topLeft = new Vector2(this.position.X - halfWidth, this.position.Y - halfHeight);
        bottomRight = new Vector2(this.position.X + halfWidth, this.position.Y + halfHeight);
        center = new Vector2(this.position.X, this.position.Y);
    }

    public void Move(Vector2 move) {
        this.position += move;
    }

    public void MoveTo(Vector2 position) {
        const float c_delta = 1f;

        if ( Vector2.Distance(this.Position, position) < c_delta ) {
            return;
        }

        this.position = position;
    }

    public void MoveZoom(float zoom) {
        this._zoom += zoom;
        if ( this._zoom < minZoom ) {
            this._zoom = minZoom;
        }

        if ( this._zoom > maxZoom ) {
            this._zoom = maxZoom;
        }

        Debug.WriteLine("Zoom: " + this._zoom);
    }

    public void ResetZoom() {
        this._zoom = this._baseZoom;
    }

    public void SetZoom(float zoom) {
        this._zoom = zoom;
        if ( this._zoom < minZoom ) {
            this._zoom = minZoom;
        }

        if ( this._zoom > maxZoom ) {
            this._zoom = maxZoom;
        }
    }

    public void UpdateProjectionMatrix() {
        this.projection = Matrix.CreatePerspectiveFieldOfView(
            fieldOfView: this.fieldOfView,
            aspectRatio: this.aspectRatio,
            nearPlaneDistance: minZoom,
            farPlaneDistance: maxZoom
        );
    }

    public void UpdateViewMatrix() {
        this.view = Matrix.CreateLookAt(
            cameraPosition: new Vector3(this.Position.X, this.Position.Y, -this._zoom),
            cameraTarget: new Vector3(this.Position.X, this.Position.Y, 0),
            cameraUpVector: Vector3.Down
        );
    }

    public void ZoomIn() {
        this.MoveZoom(-this._incrementZoom);
    }

    public void ZoomOut() {
        this.MoveZoom(this._incrementZoom);
    }

    private float _baseZoom { get; set; }
    private float _incrementZoom { get; set; }
    private float _zoom { get; set; }
    private float aspectRatio { get; set; }
    private float fieldOfView { get; set; }
    private Vector2 position { get; set; }
    private Matrix projection { get; set; }
    private Matrix view { get; set; }
}
