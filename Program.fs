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

    let b = p.AddParameterBlock [|1.0|]

    p.AddCostFunction(1, b, fun b ->
        [|
            sin b.[0]
        |]
    )

    let res = 
        p.Solve {
            print = true
            solverType = SolverType.DenseQr
            functionTolerance = 1E-20
            gradientTolerance = 1E-20
            parameterTolerance = 1E-20
            maxIterations = 100
        }

    0
