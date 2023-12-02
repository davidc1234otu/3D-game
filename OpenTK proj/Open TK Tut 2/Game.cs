using Assimp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quaternion = OpenTK.Mathematics.Quaternion;

namespace Open_TK_Tut_1;

public class Game : GameWindow
{

    private float x;

    private Shader shader;
    private Shader litShader;
    
    
    private Texture tex0;
    private Texture tex1;

    public static readonly List<GameObject> LitObjects = new();
    public static readonly List<GameObject> UnLitObjects = new();
    public static readonly List<GameObject> enemyObjects = new();
    public static readonly List<GameObject> transparentUnlitObjects = new();
    public static readonly List<PointLight> Lights = new();
    
    public static Matrix4 view;
    public static Matrix4 projection;

    public static Camera gameCam;
    private Vector2 previousMousePos;

    private string[] _pointLightDef =
    {
        "pLights[",
        "INDEX",
        "]."
    };
    

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { Title = title, Size = (width, height) })
    {
    }


    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0, 0, 0, 0);

        previousMousePos = new Vector2(MouseState.X, MouseState.Y);
        CursorState = CursorState.Grabbed;

        //Enable blending
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);
        
        shader = new Shader("shader.vert", "shader.frag");
        litShader = new Shader("shader.vert", "Lit_shader.frag");
        
        tex0 = new Texture("water.jpg");
        tex1 = new Texture("Bender.png");
        
        gameCam = new Camera(Vector3.UnitZ * 3, (float)Size.X / Size.Y);
        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\flipbook.vert", "UVTutorial\\flipbook8x8.frag")));
        transparentUnlitObjects[0].MyShader.Use();
        Texture flipbook = new Texture("Cloud03_8x8.png");
        flipbook.Use(TextureUnit.Texture2);
        GL.Uniform1(transparentUnlitObjects[0].MyShader.GetUniformLocation("tex"), 2);
        transparentUnlitObjects[0].transform.Position = new Vector3(0, 20, 0);
        transparentUnlitObjects[0].transform.Scale = new Vector3(30, 30, 30);
        transparentUnlitObjects[0].transform.Rotation = new Vector3(1,0,0);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\flipbook.vert", "UVTutorial\\flipbook2x2.frag")));
        transparentUnlitObjects[1].MyShader.Use();
        Texture flipbook1 = new Texture("rain.png");
        flipbook1.Use(TextureUnit.Texture3);
        GL.Uniform1(transparentUnlitObjects[1].MyShader.GetUniformLocation("tex"), 3);
        transparentUnlitObjects[1].transform.Position = new Vector3(-20, 12,0);
        transparentUnlitObjects[1].transform.Scale = new Vector3(20, 20, 20);
        transparentUnlitObjects[1].transform.Rotation = new Vector3(0, 90, 0);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\flipbook.vert", "UVTutorial\\flipbook2x2.frag")));
        transparentUnlitObjects[2].MyShader.Use();
        flipbook1.Use(TextureUnit.Texture3);
        GL.Uniform1(transparentUnlitObjects[2].MyShader.GetUniformLocation("tex"), 3);
        transparentUnlitObjects[2].transform.Position = new Vector3(20, 12, 0);
        transparentUnlitObjects[2].transform.Scale = new Vector3(20, 20, 20);
        transparentUnlitObjects[2].transform.Rotation = new Vector3(0, -90, 0);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\flipbook.vert", "UVTutorial\\flipbook2x2.frag")));
        transparentUnlitObjects[3].MyShader.Use();
        flipbook1.Use(TextureUnit.Texture4);
        GL.Uniform1(transparentUnlitObjects[3].MyShader.GetUniformLocation("tex"), 4);
        transparentUnlitObjects[3].transform.Position = new Vector3(0, 12, 10);
        transparentUnlitObjects[3].transform.Scale = new Vector3(20, 20, 20);
        transparentUnlitObjects[3].transform.Rotation = new Vector3(0, -90, 0);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\flipbook.vert", "UVTutorial\\flipbook2x2.frag")));
        transparentUnlitObjects[4].MyShader.Use();
        flipbook1.Use(TextureUnit.Texture5);
        GL.Uniform1(transparentUnlitObjects[4].MyShader.GetUniformLocation("tex"), 5);
        transparentUnlitObjects[4].transform.Position = new Vector3(0, 12, 10);
        transparentUnlitObjects[4].transform.Scale = new Vector3(20, 20, 20);
        transparentUnlitObjects[4].transform.Rotation = new Vector3(0, 90, 0);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\flipbook.vert", "UVTutorial\\flipbook2x2.frag")));
        transparentUnlitObjects[5].MyShader.Use();
        flipbook1.Use(TextureUnit.Texture6);
        GL.Uniform1(transparentUnlitObjects[5].MyShader.GetUniformLocation("tex"), 6);
        transparentUnlitObjects[5].transform.Position = new Vector3(0, 12, -10);
        transparentUnlitObjects[5].transform.Scale = new Vector3(20, 20, 20);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\reticle.vert", "UVTutorial\\reticle.frag")));
        transparentUnlitObjects[6].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[6].MyShader.GetUniformLocation("aspectRatio"), (float)Size.X / Size.Y);

        transparentUnlitObjects.Add(new GameObject(StaticUtilities.QuadVertices, StaticUtilities.QuadIndices,
            new Shader("UVTutorial\\reticle.vert", "UVTutorial\\reticle.frag")));
        transparentUnlitObjects[6].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[6].MyShader.GetUniformLocation("aspectRatio"), (float)Size.X / Size.Y);

        StaticUtilities.CheckError("B");


        AssimpContext importer = new AssimpContext();
        PostProcessSteps postProcessSteps = PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs;
        Scene scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "plane.fbx", postProcessSteps);
        LitObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(),
            new Shader("WaveTutorial\\Waves.vert", "Lit_shader.frag")));
        LitObjects[0].transform.Position = new Vector3(-4, -2, 0);
        LitObjects[0].transform.Rotation = Vector3.UnitX * -MathHelper.PiOver2;


        shader.Use();

        tex0.Use(TextureUnit.Texture0);
        int id = shader.GetUniformLocation("tex0");
        GL.ProgramUniform1(shader.Handle, id, 0);
        
        tex1.Use(TextureUnit.Texture1);
        id = shader.GetUniformLocation("tex1");
        GL.ProgramUniform1(shader.Handle, id, 1);

        litShader.Use(); 
        tex0.Use(TextureUnit.Texture0);
        id = litShader.GetUniformLocation("tex0");
        GL.ProgramUniform1(litShader.Handle, id, 0);
        
        tex1.Use(TextureUnit.Texture1);
        id = litShader.GetUniformLocation("tex1");
        GL.ProgramUniform1(litShader.Handle, id, 1);
        
    }

    protected override void OnUnload()
    {
        //Free GPU RAM
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        
        GL.UseProgram(0);
        
        for (int i = UnLitObjects.Count-1; i >= 0; --i)
        {
            UnLitObjects[i].Dispose();
        }
        for (int i = LitObjects.Count-1; i >= 0; --i)
        {
            LitObjects[i].Dispose();
        }
        for (int i = transparentUnlitObjects.Count - 1; i >= 0; --i)
        {
            transparentUnlitObjects[i].Dispose();
        }
        for (int i = enemyObjects.Count - 1; i >= 0; --i)
        {
            enemyObjects[i].Dispose();
        }

        shader.Dispose();

        base.OnUnload();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
    }

    public bool use = false;

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        //MUST BE FIRST
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        view = gameCam.GetViewMatrix();
        projection = gameCam.GetProjectionMatrix();
        
        
        tex0.Use(TextureUnit.Texture0);
        tex1.Use(TextureUnit.Texture1);
        
        shader.Use();
        int getCpuColor = shader.GetUniformLocation("uniformColor");
        x += (float)args.Time;
        GL.Uniform4(getCpuColor, new Color4(0, 0, x, 1.0f));
        //Console.WriteLine(getCpuColor);

        LitObjects[0].MyShader.Use();
        int idx = LitObjects[0].MyShader.GetUniformLocation("time");
        GL.Uniform1(idx, x);
        
        if (startTimer1)
        {
            enemyObjects[0].MyShader.Use();
            idx = enemyObjects[0].MyShader.GetUniformLocation("time");
            GL.Uniform1(idx, x);
        }

        if (startTimer2)
        {
            enemyObjects[1].MyShader.Use();
            idx = enemyObjects[1].MyShader.GetUniformLocation("time");
            GL.Uniform1(idx, x);
        }

        /*if (startTimer3)
        {
            enemyObjects[2].MyShader.Use();
            idx = enemyObjects[2].MyShader.GetUniformLocation("time");
            GL.Uniform1(idx, x);
        }*/

        StaticUtilities.CheckError("Can't Render White Square");
        
        foreach (GameObject unlit in UnLitObjects)
        {
            unlit.Render();
        }
        foreach (GameObject lit in LitObjects)
        {
            int id;
            lit.MyShader.Use();
            for (int i = 0; i < Lights.Count; ++i)
            {
                _pointLightDef[1] = i.ToString();
                string merged = string.Concat(_pointLightDef);

                PointLight currentLight = Lights[i];
                
                id = lit.MyShader.GetUniformLocation(merged + "lightColor");
                GL.Uniform3(id, currentLight.Color);
                id = lit.MyShader.GetUniformLocation(merged +"lightPosition");
                GL.Uniform3(id, currentLight.Transform.Position);
                id = lit.MyShader.GetUniformLocation(merged +"lightIntensity");
                GL.Uniform1(id, currentLight.Intensity);
            }

            id = lit.MyShader.GetUniformLocation("numPLights");
            GL.Uniform1(id, Lights.Count);
            lit.Render();
        }

        transparentUnlitObjects[0].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[0].MyShader.GetUniformLocation("time"), x);

        transparentUnlitObjects[1].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[1].MyShader.GetUniformLocation("time"), x);

        transparentUnlitObjects[2].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[2].MyShader.GetUniformLocation("time"), x);

        transparentUnlitObjects[3].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[3].MyShader.GetUniformLocation("time"), x);

        transparentUnlitObjects[4].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[4].MyShader.GetUniformLocation("time"), x);

        transparentUnlitObjects[5].MyShader.Use();
        GL.Uniform1(transparentUnlitObjects[5].MyShader.GetUniformLocation("time"), x);


        foreach (GameObject unlit in enemyObjects)
        {
            unlit.Render();
        }

        foreach (GameObject unlit in transparentUnlitObjects)
        {
            unlit.Render();
        }

        //MUST BE LAST
        SwapBuffers();
    }

    private float n;
    private int c, score, l = 0;
    private int lives = 100;
    private float timer;
    private bool startTimer, startTimer1, startTimer2, startTimer3 = false;
    public bool explosion, explosion1, explosion2, explosion3, notDead, gameplay = false;
    private Vector3 distance, distance1, distance2, laserDirection, objDir, objDir1, objDir2, objDir3;
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        
        if (KeyboardState.IsKeyDown(Keys.Space))
        {
            gameplay = true;
        }

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        n += (float)args.Time;
        
        //const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (use)
        {
            for (int i = enemyObjects.Count - 1; i > 0; --i)
            {
                float distanceFromTarget = Vector3.Distance(enemyObjects[i].transform.Position, LitObjects[LitObjects.Count - 1].transform.Position);
                if (distanceFromTarget <= 2.5f)
                {
                    Console.WriteLine("destroying");
                    enemyObjects.RemoveAt(i-1);
                    Lights.RemoveAt(i - 1);
                    LitObjects.RemoveAt(LitObjects.Count - 1);
                    Lights.RemoveAt(Lights.Count - 1);
                    if (i == 4)
                    {
                        //use4 = false;
                    }
                    if (i == 3)
                    {
                        startTimer3 = false;
                        //use3 = false;
                    }
                    if (i == 2)
                    {
                        startTimer2 = false;
                        //use2 = false;
                    }
                    if (i == 1)
                    {
                        startTimer1 = false;
                    }
                    if (use)
                    {
                        i = LitObjects.Count - 1;
                    }
                    startTimer = false;
                    c = 0;
                    score++;
                }
            }
        }
        else
        {
            c = 0;
        }

        Random rnd = new Random();

        if (gameplay)
        {
            if (!notDead)
            {
                if (n < 60)
                {
                    if (KeyboardState.IsKeyDown(Keys.Q))
                    {
                        AssimpContext importer = new AssimpContext();
                        PostProcessSteps postProcessSteps = PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs;
                        Scene scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "ufo.fbx", postProcessSteps);
                        enemyObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        enemyObjects[0].transform.Position = new Vector3(rnd.Next(5, 10), rnd.Next(5, 10), rnd.Next(5, 10));
                        objDir = new Vector3(-enemyObjects[0].transform.Position.X, -enemyObjects[0].transform.Position.Y, -enemyObjects[0].transform.Position.Z);
                        enemyObjects[0].transform.Rotation = new Vector3(1, 0, 0);
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer1 = true;
                        //use1 = false;

                        scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "ufo.fbx", postProcessSteps);
                        enemyObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        enemyObjects[1].transform.Position = new Vector3(rnd.Next(-10, -5), rnd.Next(5, 7), rnd.Next(5, 10));
                        objDir1 = new Vector3(-enemyObjects[1].transform.Position.X, -enemyObjects[1].transform.Position.Y, -enemyObjects[1].transform.Position.Z);
                        enemyObjects[1].transform.Rotation = new Vector3(1, 0, 0);
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer2 = true;
                        //use2 = false;

                        scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "spaceship.fbx", postProcessSteps);
                        enemyObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        enemyObjects[2].transform.Position = new Vector3(rnd.Next(5, 10), rnd.Next(5, 7), rnd.Next(5, 10));
                        objDir2 = new Vector3(-enemyObjects[2].transform.Position.X, -enemyObjects[2].transform.Position.Y, -enemyObjects[2].transform.Position.Z);
                        enemyObjects[2].transform.Rotation = new Vector3(1, 0, 0);
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer3 = true;
                        //use3 = false;
                    }


                }

                else
                {
                    if (KeyboardState.IsKeyDown(Keys.Q))
                    {
                        AssimpContext importer = new AssimpContext();
                        PostProcessSteps postProcessSteps = PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs;
                        Scene scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "ufo.fbx", postProcessSteps);
                        enemyObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        enemyObjects[0].transform.Position = new Vector3(rnd.Next(5, 10), rnd.Next(5, 10), rnd.Next(5, 10));
                        objDir = new Vector3(-enemyObjects[0].transform.Position.X, -enemyObjects[0].transform.Position.Y, -enemyObjects[0].transform.Position.Z);
                        enemyObjects[0].transform.Rotation = new Vector3(1, 0, 0);
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer1 = true;
                        //use1 = false;

                        scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "bomber.fbx", postProcessSteps);
                        enemyObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        enemyObjects[1].transform.Position = new Vector3(rnd.Next(-10, -5), rnd.Next(5, 7), rnd.Next(5, 10));
                        objDir1 = new Vector3(-enemyObjects[1].transform.Position.X, -enemyObjects[1].transform.Position.Y, -enemyObjects[1].transform.Position.Z);
                        enemyObjects[1].transform.Rotation = new Vector3(1, 0, 0);
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer2 = true;
                        //use2 = false;

                        scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "spaceship.fbx", postProcessSteps);
                        enemyObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        enemyObjects[2].transform.Position = new Vector3(rnd.Next(5, 10), rnd.Next(5, 7), rnd.Next(5, 10));
                        objDir2 = new Vector3(-enemyObjects[2].transform.Position.X, -enemyObjects[2].transform.Position.Y, -enemyObjects[2].transform.Position.Z);
                        enemyObjects[2].transform.Rotation = new Vector3(1, 0, 0);
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer3 = true;
                        //use3 = false;
                    }
                }

                if (lives == 0)
                {
                    notDead = true;
                }

                if (KeyboardState.IsKeyDown(Keys.E))
                {
                    if (c == 0)
                    {
                        Console.WriteLine("shooting");
                        AssimpContext importer = new AssimpContext();
                        PostProcessSteps postProcessSteps = PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs;
                        Scene scene = importer.ImportFile(StaticUtilities.ObjectDirectory + "laser.fbx", postProcessSteps);
                        LitObjects.Add(new GameObject(scene.Meshes[0].ConvertMesh(), scene.Meshes[0].GetUnsignedIndices(), litShader));
                        LitObjects[LitObjects.Count - 1].transform.Position = gameCam.Position;
                        LitObjects[LitObjects.Count - 1].transform.Scale = new Vector3(0.2f, 0.2f, 0.2f);
                        laserDirection.X = MathF.Cos(gameCam.Yaw * (float)Math.PI / 180);
                        laserDirection.Z = MathF.Sin(gameCam.Yaw * (float)Math.PI / 180);
                        laserDirection.Y = MathF.Sin(gameCam.Pitch * (float)Math.PI / 180);
                        LitObjects[LitObjects.Count - 1].transform.Rotation = laserDirection;
                        //l = LitObjects.Count - 1;
                        Lights.Add(new PointLight(new Vector3(1, 0, 0), 1));
                        startTimer = true;
                        use = true;
                        timer = 1.0f;
                        c++;
                    }
                }



                if (startTimer)
                {
                    LitObjects[LitObjects.Count - 1].transform.Position += laserDirection * (float)args.Time;

                    Lights[Lights.Count - 1].Transform.Position += laserDirection * (float)args.Time;
                    timer -= (float)args.Time;
                    if (timer <= 0.001)
                    {
                        LitObjects.RemoveAt(LitObjects.Count - 1);
                        Lights.RemoveAt(Lights.Count - 1);
                        startTimer = false;
                        use = false;
                        c = 0;
                    }
                }

                if (startTimer1)
                {

                    enemyObjects[0].transform.Position += objDir * (n / 1000000);

                    Lights[0].Transform.Position += objDir * (n / 1000000);
                    distance2 = enemyObjects[0].transform.Position - gameCam.Position;
                    if ((distance.X + distance.Y + distance.Z) / 3 < 0.2f)
                    {
                        enemyObjects.RemoveAt(0);
                        Lights.RemoveAt(0);
                        startTimer1 = false;
                        lives--;
                        //use1 = false;
                    }
                }

                if (startTimer2)
                {

                    enemyObjects[1].transform.Position += objDir1 * (n / 1000000);

                    Lights[1].Transform.Position += objDir1 * (n / 1000000);
                    distance2 = enemyObjects[1].transform.Position - gameCam.Position;
                    if ((distance1.X + distance1.Y + distance1.Z) / 3 < 0.2f)
                    {
                        enemyObjects.RemoveAt(1);
                        Lights.RemoveAt(1);
                        startTimer2 = false;
                        lives--;
                        //use2 = false;
                    }
                }

                if (startTimer3)
                {

                    enemyObjects[2].transform.Position += objDir2 * (n / 10000);

                    Lights[2].Transform.Position += objDir2 * (n / 10000);
                    distance2 = enemyObjects[2].transform.Position - gameCam.Position;
                    if ((distance2.X + distance2.Y + distance2.Z) / 3 < 0.2f)
                    {
                        enemyObjects.RemoveAt(2);
                        Lights.RemoveAt(2);
                        startTimer2 = false;
                        //use2 = false;
                    }
                }

                // Get the mouse state

                // Calculate the offset of the mouse position
                var deltaX = MouseState.X - previousMousePos.X;
                var deltaY = MouseState.Y - previousMousePos.Y;
                previousMousePos = new Vector2(MouseState.X, MouseState.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                gameCam.Yaw += deltaX * sensitivity;
                gameCam.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top

            }
            else
            {
                Console.WriteLine("Final Score: " + score);
                gameplay = false;
            }
        }
        
        if (!gameplay)
        {
            if (l == 0)
            {
                Console.WriteLine("Survive for as long as possible by shooting down the ships coming from the sky.");
                Console.WriteLine("Press Q to spawn enemies, E to shoot and use the Mouse to look around/aim.");
                Console.WriteLine("Press the Spacebar to start.");
                l++;
            }

        }
    }
    

protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        gameCam.Fov -= e.OffsetY;
    }
    
    
   
}