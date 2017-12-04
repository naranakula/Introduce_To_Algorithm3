cd /d E:\grpcs

windows_x86\protoc.exe -I . --csharp_out . --grpc_out . ElectService.proto HealthCheck.proto Monitor.proto --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe


protoc.exe -I E:\grpcs --csharp_out E:\grpcs\Greeter --grpc_out E:\grpcs\Greeter2 E:\grpcs\test.proto --plugin=protoc-gen-grpc=E:\grpcs\windows_x86\grpc_csharp_plugin.exe

pause