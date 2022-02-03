open Aardvark.Base
open CeresSharp

[<EntryPoint>]
let main _ =
    let oldFilter = IntrospectionProperties.UnpackNativeLibrariesFilter
    IntrospectionProperties.UnpackNativeLibrariesFilter <- System.Func<_,_>(fun a ->
        a.GetName().Name = "Ceres" || oldFilter.Invoke(a)
    )

    Aardvark.Init()
    
    use p = new Problem()

    use b = p.AddParameterBlock [|3.0|]

    p.AddCostFunction(1, b, fun b ->
        [|
            sin b.[0]
        |]
    )

    let res = 
        p.Solve {
            print = false
            solverType = SolverType.DenseQr
            functionTolerance = 1E-20
            gradientTolerance = 1E-20
            parameterTolerance = 1E-20
            maxIterations = 100
        }

    printfn "RESIDUAL: %A" res
    printfn "ERROR: %A" (sin b.Result.[0])
    printfn "RESULT: %A" b.Result.[0]

    0
