using OpenTK.Graphics.OpenGL4;

namespace Open_TK_Tut_1;

public class GameObject
{
    public Transform transform = new Transform(); // Every gameobject has a transform

    private int vertexBufferObject;
    private int vertexArrayObject;
    private int elementBufferObject;

    private readonly uint[] Indices;
    public readonly Shader MyShader;

    public GameObject(float [] vertices, uint[] indices, Shader shader)
    {
        Indices = indices; 
        MyShader = shader;
        StaticUtilities.CheckError("1");

        //VBO
        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        StaticUtilities.CheckError("2");
        //VAO
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);
        
        int id = shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(id, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(id);

        id = 1;// shader.GetAttribLocation("aNormals");
        GL.VertexAttribPointer(id, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(id);
        StaticUtilities.CheckError("3");

        id = shader.GetAttribLocation("UVs");
        GL.VertexAttribPointer(id, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(id);


        //EBO
        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices,
            BufferUsageHint.StaticDraw);
        
    }

    public void Render()
    {
        MyShader.Use();
        
        int id = MyShader.GetUniformLocation("model");
        GL.UniformMatrix4(id, true, ref transform.GetMatrix);
        id = MyShader.GetUniformLocation("view");
        GL.UniformMatrix4(id, true, ref Game.view);
        id = MyShader.GetUniformLocation("projection");
        GL.UniformMatrix4(id, true, ref Game.projection);
        
     
        id = MyShader.GetUniformLocation("viewPos");
        GL.Uniform3(id, Game.gameCam.Position);
        
        StaticUtilities.CheckError("render");
        
        
        

        GL.BindVertexArray(vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(elementBufferObject);
        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
    }



}