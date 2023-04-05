using Moq;

namespace MoviewReview.Test.Helper
{
    public static class MockExtension
    {
        public static void VerifyEverything<T>(this Mock<T> mock) where T : class
        {
            mock.VerifyAll();
            mock.VerifyNoOtherCalls();
        }

        public static void VerifyEverything<T>(this MockRepository mockRepository) where T : class
        {
            mockRepository.VerifyAll();
            mockRepository.VerifyNoOtherCalls();
        }
    }
}
