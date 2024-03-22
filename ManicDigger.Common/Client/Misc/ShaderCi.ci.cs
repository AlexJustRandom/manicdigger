﻿public class ShaderCi
{
	GamePlatform p;
	bool isLinked;
	GlProgram programId;

	public ShaderCi()
	{
		isLinked = false;
		programId = null;
		p = null;
	}

	/// <summary>
	/// Initialize the class. CiTo does not support parameters for constructors.
	/// </summary>
	/// <param name="platform"></param>
	public void Init(GamePlatform platform)
	{
		p = platform;
		programId = p.GlCreateProgram();
	}

	/// <summary>
	/// Free allocated resources on the graphics card.
	/// </summary>
	public void Dispose()
	{
		if (programId != null)
		{
			p.GlDeleteProgram(programId);
		}
		p = null;
	}

	/// <summary>
	/// Compile a specific type of shader given the source as a string.
	/// </summary>
	/// <param name="sShader">Shader source code</param>
	/// <param name="type">Shader type to create</param>
    /// returns false on unsucesful compilation;
	public bool Compile(string sShader, ShaderType type)
	{
		isLinked = false;
		GlShader shaderObject = p.GlCreateShader(type);
		if (null == shaderObject) { p.ThrowException("Could not create shader object"); }
		p.GlShaderSource(shaderObject, sShader);
		p.GlCompileShader(shaderObject);

		if (!p.GlGetShaderCompileStatus(shaderObject))
		{

    			p.ConsoleWriteLine(p.StringFormat("Error compiling shader: \n{0}", p.GlGetShaderInfoLog(shaderObject)));
            return false;
        }
		p.GlAttachShader(programId, shaderObject);
        return true;
    }

	/// <summary>
	/// Begin using this shader.
	/// </summary>
	public void BeginUse()
	{
		if (programId == null) { return; }
		p.GlUseProgram(programId);
	}

	/// <summary>
	/// End using this shader.
	/// </summary>
	public void EndUse()
	{
		if (programId == null) { return; }
		p.GlUseProgram(null);
	}


    /// <summary>
    /// TODO dumentationm?
    /// </summary>

    public void GLBindAttribLocation(int index,string name)
    {
        p.GLBindAttribLocation(programId, index, name);
    }

    /// <summary>
    /// Get location of specified uniform variable.
    /// </summary>
    /// <param name="name">Uniform variable name</param>
    /// <returns>Uniform variable location</returns>
    public int GetUniformLocation(string name)
	{
		return p.GlGetUniformLocation(programId, name);
	}

	/// <summary>
	/// Determine if the shader has been successfully linked.
	/// </summary>
	/// <returns>true if the shader is linked and ready to use, false otherwise</returns>
	public bool IsLinked()
	{
		return isLinked;
	}

	/// <summary>
	/// Link the program using previously attached shaders.
	/// </summary>
	public void Link()
	{
		p.GlLinkProgram(programId);
		if (!p.GlGetProgramLinkStatus(programId))
		{
			p.ThrowException(p.StringFormat("Error linking shader: \n{0}", p.GlGetProgramInfoLog(programId)));
		}
		isLinked = true;
	}

    //todo its stupid
    public void setBool(string name, bool value)
    {
        if(value)
            p.GlUniform1i(GetUniformLocation(name),  1);
        else
            p.GlUniform1i(GetUniformLocation(name), 0);
    }

    public void setInt(string name, int value) 
    {
        p.GlUniform1i(GetUniformLocation(name), value);

    }
    public void setFloat(string name, float value) 
    {
        p.GlUniform1f(GetUniformLocation(name), value);

    }

    public void setMat4(string name, float[] value)
    {
        p.GlUniformMatrix4(GetUniformLocation(name), value);
    }
}

public class ShaderSources
{
    public const string FragmentSolidColor =
        "#version 110\n" +
        "void main()" +
        "{" +
            "gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);" +
        "}";

    public const string VertexPassthrough =
        "#version 110\n" +
        "void main(void)" +
        "{" +
            "gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;" +
        "}";

    public const string FragmentCopy =
        "#version 130\n" +
        "uniform sampler2D image;" +
        "in vec2 uv;" +
        "void main()" +
        "{" +
            "vec3 image = texture(image, uv).rgb;" +
            "gl_FragColor = vec4(image, 1.0);" +
        "}";

    public const string VertexCreate =
        "#version 130\n" +
        "out vec2 uv;" +
        "void main()" +
        "{" +
            "const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0)," +
                                             "vec2(1.0, -1.0)," +
                                             "vec2(1.0, 1.0)," +
                                             "vec2(-1.0, 1.0));" +
            "vec2 pos = vertices[gl_VertexID];" +
            "uv = pos * 0.5 + 0.5;" +
            "gl_Position = vec4(pos, 1.0, 1.0);" +
        "}";

    public const string FragmentSkysphere =
        "#version 110\n" +
        "uniform sampler2D stars;" +
        "uniform sampler2D color;" +
        "uniform sampler2D glow;" +
        "uniform vec3 sunPos;" +
        "varying vec3 vertex;" +
        "void main()" +
        "{" +
            "vec3 V = normalize(vertex);" +
            "vec3 L = normalize(sunPos);" +
            "float vl = dot(V, L);" +
            "vec4 Kc = texture2D(color, vec2((L.y + 1.0) / 2.0, 1.0 - (V.y + 1.0) / 2.0));" +
            "vec4 Kg = texture2D(glow, vec2((L.y + 1.0) / 2.0, 1.0 - (vl + 1.0) / 2.0));" +
            "vec4 St = texture2D(stars, vec2(V.x, V.z));" +
            "gl_FragColor = mix(St, vec4(Kc.rgb + Kg.rgb * Kg.a / 2.0 , Kc.a), Kc.a);" +
        "}";

    public const string VertexSkysphere =
        "#version 110\n" +
        "varying vec3 vertex;" +
        "void main()" +
        "{" +
            "vertex = gl_Vertex.xyz;" +
            "gl_Position = ftransform();" +
        "}";

    public const string TerrainVertex =
        "#version 330 core\n" +

        "layout(location = 0) in vec3 aPos;" +
        "layout(location = 1) in vec4 aColor;" +
        "layout(location = 2) in vec2 aTexCoord;" +

        "uniform mat4 MV;"+
        "uniform mat4 P;" +

        "out vec3 ourColor;" +
        "out vec2 TexCoord;" +
         "out vec3 pos;" +

        "void main()" +
        "{" +
            "gl_Position =  MV * P * vec4(aPos, 1.0);" +
            " ourColor = aColor;"+
          "TexCoord = aTexCoord;" +
          "pos = aTexCoord;" +
        "}";
    public const string TerrainFragment =
    "#version 330 core\n" +
    "out vec4 FragColor;"+
 
    "in vec3 ourColor;"+
    "in vec2 TexCoord;"+
        "in vec3 pos;" +
    "uniform sampler2D ourTexture;" +

    "void main()"+
    "{"+
        "FragColor = vec4(255,255,1,1);"+
    "}";

 
}

public enum ShaderType
{
	VertexShader,
	FragmentShader
}
