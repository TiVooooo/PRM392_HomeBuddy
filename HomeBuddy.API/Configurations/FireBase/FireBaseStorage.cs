using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace HomeBuddy.API.Configurations.FireBase
{
    public class FireBaseStorage
    {
        public static void InitializeFirebase(IConfiguration configuration)
        {
            // Lấy đường dẫn file JSON từ appsettings.json
            var credentialFilePath = configuration["FireBaseAdmin:CredentialsFilePath"];

            // Khởi tạo FirebaseApp nếu chưa được khởi tạo
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credentialFilePath)
                });
            }
        }

        public static StorageClient CreateStorageClient(IConfiguration configuration)
        {
            // Lấy đường dẫn file JSON từ appsettings.json
            var credentialFilePath = configuration["FireBaseAdmin:CredentialsFilePath"];

            // Tạo StorageClient từ thông tin xác thực
            var credential = GoogleCredential.FromFile(credentialFilePath);
            return StorageClient.Create(credential);
        }
    }
}
