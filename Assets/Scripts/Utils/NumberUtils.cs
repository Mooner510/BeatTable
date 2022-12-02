using System;

namespace Utils {
    public static class NumberUtils {
        public static int Between(int value, int max, int min) => Math.Min(Math.Max(value, min), max);
        
        public static long Between(long value, long max, long min) => Math.Min(Math.Max(value, min), max);
        
        public static double Between(double value, double max, double min) => Math.Min(Math.Max(value, min), max);
        
        public static float Between(float value, float max, float min) => Math.Min(Math.Max(value, min), max);
    }
}