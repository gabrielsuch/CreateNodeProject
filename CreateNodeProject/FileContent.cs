using System;


namespace CreateNodeProject
{
    class FileContent
    {
        public static string AppContent()
        {
            return $@"import express, {{Request, Response}} from ""express""


const app = express()
app.use(express.json())


app.get(""/"", function(req: Request, res: Response) {{
    return res.status(200).json({{message: ""Teste""}})
}})


export default app";
        }

        public static string ServerContent()
        {
            return $@"import app from ""./app""

import {{AppDataSource}} from ""./data-source""

import dotenv from ""dotenv""

dotenv.config()


AppDataSource.initialize()
.then(() => {{
    console.log(""Data Source initialized"")

    const PORT = process.env.PORT ?? 3000

    app.listen(PORT, () => {{
        console.log(`Running on localhost:${{PORT}}`)
    }})
}})
.catch((err) => {{
    console.error(""Error during Data Source initialization"", err)
}})";
        }

        public static string DataSourceContent()
        {
            return $@"import ""reflect-metadata""

import {{DataSource}} from ""typeorm""

import dotenv from ""dotenv""
import path from ""path""

dotenv.config()


export const AppDataSource = new DataSource({{
    type: ""postgres"",
    url: process.env.DATABASE_URL,
    ssl: process.env.NODE_ENV === ""production"" ? {{rejectUnauthorized: false}}: false,
    entities: [path.join(__dirname, ""./entities/**/*.{{js,ts}}"")],
    migrations: [path.join(__dirname, ""./migrations/**/*.{{js,ts}}"")]
}})";
        }
        public static string GitIgnoreContent()
        {
            return $@".env
node_modules";
        }

        public static string DotEnvContent()
        {
            return $@"PORT=3001";
        }

        public static string UserControllerContent()
        {
            return $@"import {{Request, Response}} from ""express""

import UserService from ""../services/user.service""


class UserController {{
    createUser = async (req: Request, res: Response) => {{
        const user = await UserService.createUser(req)

        return res.status(user.status).json(user.message)
    }}
}}


export default new UserController()";
        }

        public static string UserServiceContent()
        {
            return $@"import {{Request}} from ""express""


class UserService {{
    createUser = async ({{body}}: Request): Promise<{{status: number, message: typeof body}}> => {{
        return {{status: 201, message: body}}
    }}
}}


export default new UserService()";
        }

        public static string IndexRouteContent()
        {
            return $@"import {{Express}} from ""express""

import userRoute from ""./user.route""


const registerRoutes = (app: Express) => {{
    app.use(""/user"", userRoute())
}}


export default registerRoutes";
        }

        public static string UserRouteContent()
        {
            return $@"import {{Router}} from ""express""

import UserController from ""../controllers/user.controller""


const route = Router()


const userRoute = () => {{
    route.post("""", UserController.createUser)

    return route
}}


export default userRoute";
        }

        public static string PackageJsonMainContent(string fileType)
        {
            return $@"""main"": ""./src/server.{fileType}""";
        }

        public static string PackageJsonScriptsContent(string fileType)
        {
            return $@"""dev"": ""ts-node-dev src/server.{fileType}"",
    ""start"": ""node dist/server.{fileType}"",
    ""build"": ""tsc"",
    ""typeorm"": ""typeorm-ts-node-commonjs"",
    ""migration:run"": ""typeorm-ts-node-commonjs migration:run -d src/data-source.{fileType}""";
        }
    }
}
