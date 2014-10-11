@if "%DevEnvDir%"=="" goto error_no_DevEnvDir

copy TransformCodeGenerator.dll "%DevEnvDir%\PrivateAssemblies\"
regasm "%DevEnvDir%\PrivateAssemblies\TransformCodeGenerator.dll"

@goto end

:error_no_DevEnvDir
@echo ERROR: DevEnvDir variable is not set. 
@goto end

:end
