using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_PR3
{
   public class Cube
   {
      private int _vertexBufferObject;
      private int _vaoModel;
      private Shader _lightingShader;
      private Vector3[] _points =
      {
         new (-0.5f, -0.5f, -0.5f),
         new ( 0.5f, -0.5f, -0.5f),
         new (-0.5f, 0.5f,  -0.5f),
         new ( 0.5f, 0.5f,  -0.5f),

         new (-0.5f, -0.5f, 0.5f),
         new ( 0.5f, -0.5f, 0.5f),
         new (-0.5f, 0.5f,  0.5f),
         new ( 0.5f, 0.5f,  0.5f)
      };
      private float[] _vertices;

      private float[] _FrameVertices;
      private int _vboFrame;
      private int _vaoFrame;

      Shader _normalShader;
      private int _vboNormals;
      private int _vaoNormals;
      private float[] _normalLines;


      private float _scale;
      private Vector3 _axis;
      private float _angle;

      public bool IsNormalDefault;

      public Vector3 Position { get; set; }

      public Cube(Shader lightingShader, Shader normalShader, Vector3 position)
      {
         _scale = 1.0f;
         _angle = 0.0f;
         _axis = new Vector3(0.0f);


         Position = position;
         _lightingShader = lightingShader;
         _normalShader = normalShader;
         SetDefaultNormals();
         SetFrameVertices();
         IsNormalDefault = true;

         _vertexBufferObject = GL.GenBuffer();
         GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
         GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

         _vaoModel = GL.GenVertexArray();
         GL.BindVertexArray(_vaoModel);

         var positionLocation = _lightingShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

         var normalLocation = _lightingShader.GetAttribLocation("aNormal");
         GL.EnableVertexAttribArray(normalLocation);
         GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

         var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
         GL.EnableVertexAttribArray(texCoordLocation);
         GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

         Matrix4 model = Matrix4.CreateTranslation(Position);
         _lightingShader.SetMatrix4("model", model);



         _vboNormals = GL.GenBuffer();
         GL.BindBuffer(BufferTarget.ArrayBuffer, _vboNormals);
         GL.BufferData(BufferTarget.ArrayBuffer, _normalLines.Length * sizeof(float), _normalLines, BufferUsageHint.StaticDraw);

         _vaoNormals = GL.GenVertexArray();
         GL.BindVertexArray(_vaoNormals);

         var positionLocation2 = _normalShader.GetAttribLocation("nPos");
         GL.EnableVertexAttribArray(positionLocation2);
         GL.VertexAttribPointer(positionLocation2, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

         var model2 = Matrix4.CreateTranslation(Position);
         _normalShader.SetMatrix4("nmodel", model2);
         GL.BindVertexArray(0);
      }

      public void Render(bool frameOnly = false)
      {
         GL.BindVertexArray(_vaoModel);
         if (frameOnly is false)
         {
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 8);
         }
         else
         {
            GL.BindVertexArray(_vaoFrame);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, 22);
            GL.BindVertexArray(0);
         }
      }

      private void SetFrameVertices()
      {
         _FrameVertices = new float[]
         {
            _points[0].X, _points[0].Y, _points[0].Z,
            _points[1].X, _points[1].Y, _points[1].Z,
            _points[2].X, _points[2].Y, _points[2].Z,
            _points[0].X, _points[0].Y, _points[0].Z,
            _points[1].X, _points[1].Y, _points[1].Z,
            _points[3].X, _points[3].Y, _points[3].Z,
            _points[2].X, _points[2].Y, _points[2].Z,

            _points[6].X, _points[6].Y, _points[6].Z,
            _points[4].X, _points[4].Y, _points[4].Z,
            _points[0].X, _points[0].Y, _points[0].Z,
            _points[6].X, _points[6].Y, _points[6].Z,

            _points[7].X, _points[7].Y, _points[7].Z,
            _points[4].X, _points[4].Y, _points[4].Z,
            _points[5].X, _points[5].Y, _points[5].Z,
            _points[7].X, _points[7].Y, _points[7].Z,

            _points[3].X, _points[3].Y, _points[3].Z,
            _points[5].X, _points[5].Y, _points[5].Z,
            _points[1].X, _points[1].Y, _points[1].Z,
            _points[3].X, _points[3].Y, _points[3].Z,

            _points[6].X, _points[6].Y, _points[6].Z,
            _points[4].X, _points[4].Y, _points[4].Z,
            _points[1].X, _points[1].Y, _points[1].Z,
         };

         _vboFrame = GL.GenBuffer();
         GL.BindBuffer(BufferTarget.ArrayBuffer, _vboFrame);
         GL.BufferData(BufferTarget.ArrayBuffer, _FrameVertices.Length * sizeof(float), _FrameVertices, BufferUsageHint.StreamDraw);

         _vaoFrame = GL.GenVertexArray();
         GL.BindVertexArray(_vaoFrame);

         var positionLocation = _lightingShader.GetAttribLocation("aPos");
         GL.EnableVertexAttribArray(positionLocation);
         GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

         Matrix4 model = Matrix4.CreateTranslation(Position);
         _lightingShader.SetMatrix4("model", model);

         GL.BindVertexArray(0);
         GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }

      public void DrawNormals()
      {
         GL.BindVertexArray(_vaoNormals);
         GL.DrawArrays(PrimitiveType.Lines, 0, _normalLines.Length / 3);
      }

      public void Scale(float scale)
      {
         Matrix4 model = Matrix4.CreateTranslation(Position);
         model *= Matrix4.CreateScale(_scale);
         //model *= Matrix4.CreateFromAxisAngle(_axis, _angle);

         _scale = scale;
         model *= Matrix4.CreateScale(scale);
         _lightingShader.SetMatrix4("model", model);
      }

      public void Rotate(Vector3 axis, float angle)
      {
         Matrix4 model = Matrix4.CreateTranslation(Position);
         model *= Matrix4.CreateScale(_scale);
         //model *= Matrix4.CreateFromAxisAngle(_axis, _angle);

         (_axis, _angle) = (axis, angle);
         model *= Matrix4.CreateFromAxisAngle(axis, angle);
         _lightingShader.SetMatrix4("model", model);
      }

      public void ScaleNormalsAsCube()
      {
         Matrix4 model = Matrix4.CreateTranslation(Position);
         model *= Matrix4.CreateScale(_scale);
         model *= Matrix4.CreateFromAxisAngle(_axis, _angle);

         _lightingShader.SetMatrix4("model", model);
      }

      public void RotateNormalsAsCube()
      {
         Matrix4 model = Matrix4.CreateTranslation(Position);
         model *= Matrix4.CreateScale(_scale);
         model *= Matrix4.CreateFromAxisAngle(_axis, _angle);

         _normalShader.SetMatrix4("nmodel", model);
      }

      public void SetDefaultNormals()
      {
         IsNormalDefault = true;
         Vector3[] normals =
         {
            //{ 0, 1, 2, 1, 3, 2 },  // перед
            Normal(_points[0], _points[1], _points[2]),

            //{ 2, 3, 6, 3, 7, 6 },  // верх
            Normal(_points[2], _points[3], _points[6]),

            //{ 4, 5, 0, 5, 1, 0 },  // низ
            Normal(_points[4], _points[5], _points[0]),

            //{ 4, 0, 6, 0, 2, 6 },  // лево
            Normal(_points[4], _points[0], _points[6]),

            //{ 1, 5, 3, 5, 7, 3 },  // право
            Normal(_points[1], _points[5], _points[3]),

            //{ 5, 4, 7, 4, 6, 7 }   // зад
            Normal(_points[5], _points[4], _points[7])
         };


         for (int i = 0; i < normals.Length; i++) 
         {
            normals[i].Normalize();
         }

         _vertices = new float[]
         {
            //{ 0, 1, 2, 1, 3, 2 },          Нормаль                          // перед
            _points[0].X, _points[0].Y, _points[0].Z, normals[0].X, normals[0].Y, normals[0].Z, 0.33f, 0.0f,
            _points[1].X, _points[1].Y, _points[1].Z, normals[0].X, normals[0].Y, normals[0].Z, 0.67f, 0.0f,
            _points[2].X, _points[2].Y, _points[2].Z, normals[0].X, normals[0].Y, normals[0].Z, 0.33f, 0.33f,
            _points[1].X, _points[1].Y, _points[1].Z, normals[0].X, normals[0].Y, normals[0].Z, 0.67f, 0.0f,
            _points[3].X, _points[3].Y, _points[3].Z, normals[0].X, normals[0].Y, normals[0].Z, 0.67f, 0.33f,
            _points[2].X, _points[2].Y, _points[2].Z, normals[0].X, normals[0].Y, normals[0].Z, 0.33f, 0.33f,

            //{ 2, 3, 6, 3, 7, 6 },          Нормаль                          // верх
            _points[2].X, _points[2].Y, _points[2].Z, normals[1].X, normals[1].Y, normals[1].Z, 0.33f, 0.33f,
            _points[3].X, _points[3].Y, _points[3].Z, normals[1].X, normals[1].Y, normals[1].Z, 0.67f, 0.33f,
            _points[6].X, _points[6].Y, _points[6].Z, normals[1].X, normals[1].Y, normals[1].Z, 0.33f, 0.67f,
            _points[3].X, _points[3].Y, _points[3].Z, normals[1].X, normals[1].Y, normals[1].Z, 0.67f, 0.33f,
            _points[7].X, _points[7].Y, _points[7].Z, normals[1].X, normals[1].Y, normals[1].Z, 0.67f, 0.67f,
            _points[6].X, _points[6].Y, _points[6].Z, normals[1].X, normals[1].Y, normals[1].Z, 0.33f, 0.67f,

            //{ 4, 5, 0, 5, 1, 0 },          Нормаль                          // низ
            _points[4].X, _points[4].Y, _points[4].Z, normals[2].X, normals[2].Y, normals[2].Z, 0.33f, 0.33f,
            _points[5].X, _points[5].Y, _points[5].Z, normals[2].X, normals[2].Y, normals[2].Z, 0.67f, 0.33f,
            _points[0].X, _points[0].Y, _points[0].Z, normals[2].X, normals[2].Y, normals[2].Z, 0.33f, 0.67f,
            _points[5].X, _points[5].Y, _points[5].Z, normals[2].X, normals[2].Y, normals[2].Z, 0.67f, 0.33f,
            _points[1].X, _points[1].Y, _points[1].Z, normals[2].X, normals[2].Y, normals[2].Z, 0.67f, 0.67f,
            _points[0].X, _points[0].Y, _points[0].Z, normals[2].X, normals[2].Y, normals[2].Z, 0.33f, 0.67f,

            //{ 4, 0, 6, 0, 2, 6 },          Нормаль                          // лево
            _points[4].X, _points[4].Y, _points[4].Z, normals[3].X, normals[3].Y, normals[3].Z, 0.0f, 0.0f,
            _points[0].X, _points[0].Y, _points[0].Z, normals[3].X, normals[3].Y, normals[3].Z, 0.33f, 0.0f,
            _points[6].X, _points[6].Y, _points[6].Z, normals[3].X, normals[3].Y, normals[3].Z, 0.0f, 0.33f,
            _points[0].X, _points[0].Y, _points[0].Z, normals[3].X, normals[3].Y, normals[3].Z, 0.33f, 0.0f,
            _points[2].X, _points[2].Y, _points[2].Z, normals[3].X, normals[3].Y, normals[3].Z, 0.33f, 0.33f,
            _points[6].X, _points[6].Y, _points[6].Z, normals[3].X, normals[3].Y, normals[3].Z, 0.0f, 0.33f,

            //{ 1, 5, 3, 5, 7, 3 },          Нормаль                          // право
            _points[1].X, _points[1].Y, _points[1].Z, normals[4].X, normals[4].Y, normals[4].Z, 0.67f, 0.0f,
            _points[5].X, _points[5].Y, _points[5].Z, normals[4].X, normals[4].Y, normals[4].Z, 1.0f, 0.0f,
            _points[3].X, _points[3].Y, _points[3].Z, normals[4].X, normals[4].Y, normals[4].Z, 0.67f, 0.33f,
            _points[5].X, _points[5].Y, _points[5].Z, normals[4].X, normals[4].Y, normals[4].Z, 1.0f, 0.0f,
            _points[7].X, _points[7].Y, _points[7].Z, normals[4].X, normals[4].Y, normals[4].Z, 1.0f, 0.33f,
            _points[3].X, _points[3].Y, _points[3].Z, normals[4].X, normals[4].Y, normals[4].Z, 0.67f, 0.33f,

            //{ 5, 4, 7, 4, 6, 7 }           Нормаль                          // зад
            _points[5].X, _points[5].Y, _points[5].Z, normals[5].X, normals[5].Y, normals[5].Z, 0.33f, 0.67f,
            _points[4].X, _points[4].Y, _points[4].Z, normals[5].X, normals[5].Y, normals[5].Z, 0.67f, 0.67f,
            _points[7].X, _points[7].Y, _points[7].Z, normals[5].X, normals[5].Y, normals[5].Z, 0.33f, 1.0f,
            _points[4].X, _points[4].Y, _points[4].Z, normals[5].X, normals[5].Y, normals[5].Z, 0.67f, 0.67f,
            _points[6].X, _points[6].Y, _points[6].Z, normals[5].X, normals[5].Y, normals[5].Z, 0.67f, 1.0f,
            _points[7].X, _points[7].Y, _points[7].Z, normals[5].X, normals[5].Y, normals[5].Z, 0.33f, 1.0f
         };

         _normalLines = new float[]
         {
            _points[0].X, _points[0].Y, _points[0].Z, _points[0].X + normals[0].X, _points[0].Y + normals[0].Y, _points[0].Z + normals[0].Z,
            _points[0].X, _points[0].Y, _points[0].Z, _points[0].X + normals[2].X, _points[0].Y + normals[2].Y, _points[0].Z + normals[2].Z,
            _points[0].X, _points[0].Y, _points[0].Z, _points[0].X + normals[3].X, _points[0].Y + normals[3].Y, _points[0].Z + normals[3].Z,

            _points[1].X, _points[1].Y, _points[1].Z, _points[1].X + normals[0].X, _points[1].Y + normals[0].Y, _points[1].Z + normals[0].Z,
            _points[1].X, _points[1].Y, _points[1].Z, _points[1].X + normals[2].X, _points[1].Y + normals[2].Y, _points[1].Z + normals[2].Z,
            _points[1].X, _points[1].Y, _points[1].Z, _points[1].X + normals[4].X, _points[1].Y + normals[4].Y, _points[1].Z + normals[4].Z,

            _points[2].X, _points[2].Y, _points[2].Z, _points[2].X + normals[0].X, _points[2].Y + normals[0].Y, _points[2].Z + normals[0].Z,
            _points[2].X, _points[2].Y, _points[2].Z, _points[2].X + normals[1].X, _points[2].Y + normals[1].Y, _points[2].Z + normals[1].Z,
            _points[2].X, _points[2].Y, _points[2].Z, _points[2].X + normals[3].X, _points[2].Y + normals[3].Y, _points[2].Z + normals[3].Z,

            _points[3].X, _points[3].Y, _points[3].Z, _points[3].X + normals[0].X, _points[3].Y + normals[0].Y, _points[3].Z + normals[0].Z,
            _points[3].X, _points[3].Y, _points[3].Z, _points[3].X + normals[1].X, _points[3].Y + normals[1].Y, _points[3].Z + normals[1].Z,
            _points[3].X, _points[3].Y, _points[3].Z, _points[3].X + normals[4].X, _points[3].Y + normals[4].Y, _points[3].Z + normals[4].Z,
                                                                                                                           
            _points[4].X, _points[4].Y, _points[4].Z, _points[4].X + normals[2].X, _points[4].Y + normals[2].Y, _points[4].Z + normals[2].Z,
            _points[4].X, _points[4].Y, _points[4].Z, _points[4].X + normals[3].X, _points[4].Y + normals[3].Y, _points[4].Z + normals[3].Z,
            _points[4].X, _points[4].Y, _points[4].Z, _points[4].X + normals[5].X, _points[4].Y + normals[5].Y, _points[4].Z + normals[5].Z,
                                                                                                                           
            _points[5].X, _points[5].Y, _points[5].Z, _points[5].X + normals[2].X, _points[5].Y + normals[2].Y, _points[5].Z + normals[2].Z,
            _points[5].X, _points[5].Y, _points[5].Z, _points[5].X + normals[4].X, _points[5].Y + normals[4].Y, _points[5].Z + normals[4].Z,
            _points[5].X, _points[5].Y, _points[5].Z, _points[5].X + normals[5].X, _points[5].Y + normals[5].Y, _points[5].Z + normals[5].Z,
                                                                                                                           
            _points[6].X, _points[6].Y, _points[6].Z, _points[6].X + normals[1].X, _points[6].Y + normals[1].Y, _points[6].Z + normals[1].Z,
            _points[6].X, _points[6].Y, _points[6].Z, _points[6].X + normals[3].X, _points[6].Y + normals[3].Y, _points[6].Z + normals[3].Z,
            _points[6].X, _points[6].Y, _points[6].Z, _points[6].X + normals[5].X, _points[6].Y + normals[5].Y, _points[6].Z + normals[5].Z,
                                                                                                                           
            _points[7].X, _points[7].Y, _points[7].Z, _points[7].X + normals[1].X, _points[7].Y + normals[1].Y, _points[7].Z + normals[1].Z,
            _points[7].X, _points[7].Y, _points[7].Z, _points[7].X + normals[4].X, _points[7].Y + normals[4].Y, _points[7].Z + normals[4].Z,
            _points[7].X, _points[7].Y, _points[7].Z, _points[7].X + normals[5].X, _points[7].Y + normals[5].Y, _points[7].Z + normals[5].Z
         };

         GL.BindBuffer(BufferTarget.ArrayBuffer, _vboNormals);
         GL.BufferData(BufferTarget.ArrayBuffer, _normalLines.Length * sizeof(float), _normalLines, BufferUsageHint.StaticDraw);

         GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
         GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

         GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }

      public void SetSmoothedNormals()
      {
         IsNormalDefault = false;
         Vector3[] smoothNormals =
   {
            new(-1.0f, -1.0f, -1.0f),
            new(1.0f, -1.0f, -1.0f),
            new(-1.0f, 1.0f, -1.0f),
            new(1.0f, 1.0f, -1.0f),
            new(-1.0f, -1.0f, 1.0f),
            new(1.0f, -1.0f, 1.0f),
            new(-1.0f, 1.0f, 1.0f),
            new(1.0f, 1.0f, 1.0f),
         };

         for (int i = 0; i < smoothNormals.Length; i++)
         {
            smoothNormals[i].Normalize();
         }

         _vertices = new float[]
         {
            //{ 0, 1, 2, 1, 3, 2 },          Нормаль                          // перед
            _points[0].X, _points[0].Y, _points[0].Z, smoothNormals[0].X, smoothNormals[0].Y, smoothNormals[0].Z, 0.33f, 0.0f,  
            _points[1].X, _points[1].Y, _points[1].Z, smoothNormals[1].X, smoothNormals[1].Y, smoothNormals[1].Z, 0.67f, 0.0f,
            _points[2].X, _points[2].Y, _points[2].Z, smoothNormals[2].X, smoothNormals[2].Y, smoothNormals[2].Z, 0.33f, 0.33f,
            _points[1].X, _points[1].Y, _points[1].Z, smoothNormals[1].X, smoothNormals[1].Y, smoothNormals[1].Z, 0.67f, 0.0f,
            _points[3].X, _points[3].Y, _points[3].Z, smoothNormals[3].X, smoothNormals[3].Y, smoothNormals[3].Z, 0.67f, 0.33f,
            _points[2].X, _points[2].Y, _points[2].Z, smoothNormals[2].X, smoothNormals[2].Y, smoothNormals[2].Z, 0.33f, 0.33f,
            
            //{ 2, 3, 6, 3, 7, 6 },          Нормаль                          // верх
            _points[2].X, _points[2].Y, _points[2].Z, smoothNormals[2].X, smoothNormals[2].Y, smoothNormals[2].Z, 0.33f, 0.33f,
            _points[3].X, _points[3].Y, _points[3].Z, smoothNormals[3].X, smoothNormals[3].Y, smoothNormals[3].Z, 0.67f, 0.33f,
            _points[6].X, _points[6].Y, _points[6].Z, smoothNormals[6].X, smoothNormals[6].Y, smoothNormals[6].Z, 0.33f, 0.67f,
            _points[3].X, _points[3].Y, _points[3].Z, smoothNormals[3].X, smoothNormals[3].Y, smoothNormals[3].Z, 0.67f, 0.33f,
            _points[7].X, _points[7].Y, _points[7].Z, smoothNormals[7].X, smoothNormals[7].Y, smoothNormals[7].Z, 0.67f, 0.67f,
            _points[6].X, _points[6].Y, _points[6].Z, smoothNormals[6].X, smoothNormals[6].Y, smoothNormals[6].Z, 0.33f, 0.67f,
            
            //{ 4, 5, 0, 5, 1, 0 },          Нормаль                          // низ
            _points[4].X, _points[4].Y, _points[4].Z, smoothNormals[4].X, smoothNormals[4].Y, smoothNormals[4].Z, 0.33f, 0.33f,
            _points[5].X, _points[5].Y, _points[5].Z, smoothNormals[5].X, smoothNormals[5].Y, smoothNormals[5].Z, 0.67f, 0.33f,
            _points[0].X, _points[0].Y, _points[0].Z, smoothNormals[0].X, smoothNormals[0].Y, smoothNormals[0].Z, 0.33f, 0.67f,
            _points[5].X, _points[5].Y, _points[5].Z, smoothNormals[5].X, smoothNormals[5].Y, smoothNormals[5].Z, 0.67f, 0.33f,
            _points[1].X, _points[1].Y, _points[1].Z, smoothNormals[1].X, smoothNormals[1].Y, smoothNormals[1].Z, 0.67f, 0.67f,
            _points[0].X, _points[0].Y, _points[0].Z, smoothNormals[0].X, smoothNormals[0].Y, smoothNormals[0].Z, 0.33f, 0.67f,
            
            //{ 4, 0, 6, 0, 2, 6 },          Нормаль                          // лево
            _points[4].X, _points[4].Y, _points[4].Z, smoothNormals[4].X, smoothNormals[4].Y, smoothNormals[4].Z, 0.0f, 0.0f,
            _points[0].X, _points[0].Y, _points[0].Z, smoothNormals[0].X, smoothNormals[0].Y, smoothNormals[0].Z, 0.33f, 0.0f,
            _points[6].X, _points[6].Y, _points[6].Z, smoothNormals[6].X, smoothNormals[6].Y, smoothNormals[6].Z, 0.0f, 0.33f,
            _points[0].X, _points[0].Y, _points[0].Z, smoothNormals[0].X, smoothNormals[0].Y, smoothNormals[0].Z, 0.33f, 0.0f,
            _points[2].X, _points[2].Y, _points[2].Z, smoothNormals[2].X, smoothNormals[2].Y, smoothNormals[2].Z, 0.33f, 0.33f,
            _points[6].X, _points[6].Y, _points[6].Z, smoothNormals[6].X, smoothNormals[6].Y, smoothNormals[6].Z, 0.0f, 0.33f,
            
            //{ 1, 5, 3, 5, 7, 3 },          Нормаль                          // право
            _points[1].X, _points[1].Y, _points[1].Z, smoothNormals[1].X, smoothNormals[1].Y, smoothNormals[1].Z, 0.67f, 0.0f,
            _points[5].X, _points[5].Y, _points[5].Z, smoothNormals[5].X, smoothNormals[5].Y, smoothNormals[5].Z, 1.0f, 0.0f,
            _points[3].X, _points[3].Y, _points[3].Z, smoothNormals[3].X, smoothNormals[3].Y, smoothNormals[3].Z, 0.67f, 0.33f,
            _points[5].X, _points[5].Y, _points[5].Z, smoothNormals[5].X, smoothNormals[5].Y, smoothNormals[5].Z, 1.0f, 0.0f,
            _points[7].X, _points[7].Y, _points[7].Z, smoothNormals[7].X, smoothNormals[7].Y, smoothNormals[7].Z, 1.0f, 0.33f,
            _points[3].X, _points[3].Y, _points[3].Z, smoothNormals[3].X, smoothNormals[3].Y, smoothNormals[3].Z, 0.67f, 0.33f,
            
            //{ 5, 4, 7, 4, 6, 7 }           Нормаль                          // зад
            _points[5].X, _points[5].Y, _points[5].Z, smoothNormals[5].X, smoothNormals[5].Y, smoothNormals[5].Z, 0.33f, 0.67f,
            _points[4].X, _points[4].Y, _points[4].Z, smoothNormals[4].X, smoothNormals[4].Y, smoothNormals[4].Z, 0.67f, 0.67f,
            _points[7].X, _points[7].Y, _points[7].Z, smoothNormals[7].X, smoothNormals[7].Y, smoothNormals[7].Z, 0.33f, 1.0f,
            _points[4].X, _points[4].Y, _points[4].Z, smoothNormals[4].X, smoothNormals[4].Y, smoothNormals[4].Z, 0.67f, 0.67f,
            _points[6].X, _points[6].Y, _points[6].Z, smoothNormals[6].X, smoothNormals[6].Y, smoothNormals[6].Z, 0.67f, 1.0f,
            _points[7].X, _points[7].Y, _points[7].Z, smoothNormals[7].X, smoothNormals[7].Y, smoothNormals[7].Z, 0.33f, 1.0f
         };


         _normalLines = new float[]
         {
            _points[0].X, _points[0].Y, _points[0].Z, _points[0].X + smoothNormals[0].X, _points[0].Y + smoothNormals[0].Y, _points[0].Z + smoothNormals[0].Z,
            _points[1].X, _points[1].Y, _points[1].Z, _points[1].X + smoothNormals[1].X, _points[1].Y + smoothNormals[1].Y, _points[1].Z + smoothNormals[1].Z,
            _points[2].X, _points[2].Y, _points[2].Z, _points[2].X + smoothNormals[2].X, _points[2].Y + smoothNormals[2].Y, _points[2].Z + smoothNormals[2].Z,
            _points[3].X, _points[3].Y, _points[3].Z, _points[3].X + smoothNormals[3].X, _points[3].Y + smoothNormals[3].Y, _points[3].Z + smoothNormals[3].Z,
            _points[4].X, _points[4].Y, _points[4].Z, _points[4].X + smoothNormals[4].X, _points[4].Y + smoothNormals[4].Y, _points[4].Z + smoothNormals[4].Z,
            _points[5].X, _points[5].Y, _points[5].Z, _points[5].X + smoothNormals[5].X, _points[5].Y + smoothNormals[5].Y, _points[5].Z + smoothNormals[5].Z,
            _points[6].X, _points[6].Y, _points[6].Z, _points[6].X + smoothNormals[6].X, _points[6].Y + smoothNormals[6].Y, _points[6].Z + smoothNormals[6].Z,
            _points[7].X, _points[7].Y, _points[7].Z, _points[7].X + smoothNormals[7].X, _points[7].Y + smoothNormals[7].Y, _points[7].Z + smoothNormals[7].Z
         };

         GL.BindBuffer(BufferTarget.ArrayBuffer, _vboNormals);
         GL.BufferData(BufferTarget.ArrayBuffer, _normalLines.Length * sizeof(float), _normalLines, BufferUsageHint.StaticDraw);

         GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
         GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

         GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      }

      private Vector3 Normal(Vector3 v1, Vector3 v2, Vector3 v3)
      {
         Vector3 u = Vector3.Subtract(v2, v1);
         Vector3 v = Vector3.Subtract(v3, v1);

         Vector3 normal = Vector3.Cross(v, u);
         normal.Normalize();
         return normal;
      }

   }
}
