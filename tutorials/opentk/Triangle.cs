using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class Triangle : GameWindow
{
    private int vertexbuffer;
    private int shaderProgram;
    private int vertexArray;


    public Triangle()
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(800, 600));

    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }
    protected override void OnLoad()
    {
        GL.ClearColor(Color4.Black);

        float[] vertices = new float[]
        {
            0.0f,0.5f,0f,
            0.5f,-0.5f,0f,
            -0.5f,-0.5f,0f
        };

        vertexbuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        vertexArray = GL.GenVertexArray();
        GL.BindVertexArray(vertexArray);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);

        string vertexShaderCode =
            @"
                #version 330 core

                layout (location=0) in vec3 aPosition;
                 
                void main(){

                    gl_Position=vec4(aPosition,1.0f);

                }";

        string pixelShaderCode =
            @"
                #version 330 core

                out vec4 color;

                void main(){
                    color=vec4(1f,0.0f,0.0f,1.0f); // red triangle
                }
                ";

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexbuffer, vertexShaderCode);
        GL.CompileShader(vertexShader);


        int pixelShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(pixelShader, pixelShaderCode);
        GL.CompileShader(pixelShader);

        shaderProgram = GL.CreateProgram();

        GL.AttachShader(this.shaderProgram, vertexShader);
        GL.AttachShader(this.shaderProgram, pixelShader);

        GL.LinkProgram(this.shaderProgram);

        GL.DetachShader(this.shaderProgram, vertexShader);
        GL.DetachShader(this.shaderProgram, pixelShader);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(pixelShader);

        base.OnLoad();
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(vertexbuffer);

        GL.UseProgram(0);
        GL.DeleteProgram(shaderProgram);

        base.OnUnload();
    }
    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
        base.OnResize(e);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.BindVertexArray(vertexArray);

        GL.UseProgram(shaderProgram);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();

        base.OnRenderFrame(args);
    }
}