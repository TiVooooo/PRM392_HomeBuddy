using Firebase.Auth;
using Firebase.Storage;
using HomeBuddy.Service.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth.Providers;

namespace HomeBuddy.Service.Services
{
    public interface IFirebaseService
    {
        Task<string> UploadImageToFirebaseAsync(IFormFile imageFile, string basePath);
        Task<bool> DeleteFileFromFirebase(string fileUrl);
    }

    public class FirebaseService : IFirebaseService
    {
        private readonly FireBaseConfigurationModel _firebaseConfig;

        public FirebaseService(IOptions<FireBaseConfigurationModel> firebaseConfig)
        {
            _firebaseConfig = firebaseConfig.Value;
        }

        
        public async Task<string> UploadImageToFirebaseAsync(IFormFile imageFile, string basePath)
        {
            if (imageFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var destinationPath = $"{basePath}/{fileName}";

                var config = new FirebaseAuthConfig
                {
                    ApiKey = _firebaseConfig.ApiKey,
                    AuthDomain = _firebaseConfig.AuthDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider(),
                        new GoogleProvider()
                    }
                };

                var authClient = new FirebaseAuthClient(config);
                var userCredential = await authClient.SignInWithEmailAndPasswordAsync(_firebaseConfig.AuthEmail, _firebaseConfig.AuthPassword);

                if (userCredential == null)
                    throw new FirebaseAuthException("Không thể xác thực người dùng.", new AuthErrorReason());

                var storage = new FirebaseStorage(
                    _firebaseConfig.StorageBucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () => await userCredential.User.GetIdTokenAsync(),
                        ThrowOnCancel = true
                    });

                using var stream = imageFile.OpenReadStream();
                var downloadUrl = await storage.Child(destinationPath).PutAsync(stream);

                return downloadUrl;
            }

            return null;
        }

        public async Task<bool> DeleteFileFromFirebase(string fileUrl)
        {
            try
            {
                var filePath = ExtractFilePathFromUrl(fileUrl);

                var config = new FirebaseAuthConfig
                {
                    ApiKey = _firebaseConfig.ApiKey,
                    AuthDomain = _firebaseConfig.AuthDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider(),
                        new GoogleProvider()
                    }
                };

                var authClient = new FirebaseAuthClient(config);
                var userCredential = await authClient.SignInWithEmailAndPasswordAsync(_firebaseConfig.AuthEmail, _firebaseConfig.AuthPassword);

                if (userCredential == null)
                    throw new FirebaseAuthException("Không thể xác thực người dùng.", new AuthErrorReason());

                var storage = new FirebaseStorage(
                    _firebaseConfig.StorageBucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () => await userCredential.User.GetIdTokenAsync(),
                        ThrowOnCancel = true
                    });

                await storage.Child(filePath).DeleteAsync();
                return true;
            }
            catch (FirebaseStorageException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                return false;
            }
        }

        
        private string ExtractFilePathFromUrl(string fileUrl)
        {
            var startIndex = fileUrl.IndexOf("/o/") + 3;
            var endIndex = fileUrl.IndexOf("?");

            if (startIndex >= 0 && endIndex > startIndex)
            {
                var filePath = fileUrl.Substring(startIndex, endIndex - startIndex);
                return Uri.UnescapeDataString(filePath);
            }

            throw new ArgumentException("Định dạng URL file không hợp lệ.");
        }
    }
}