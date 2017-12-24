namespace Progression.Resource {
    public enum ResourceTypeEnum : uint
    {
        
        
        //Text
        CategoryText = ResourceCategoryEnum.Text << 16,
        Text = CategoryText | 0x00000001,
        KeyValue = CategoryText | 0x00000002,
        TextOther = CategoryText | MaskType,
        
        
        //Graphics
        CategoryGraphics = ResourceCategoryEnum.Graphics << 16,
        BitmapImage = CategoryGraphics | 0x00000001,
        VectorImage = CategoryGraphics | 0x00000002,
        GraphicsOther = CategoryGraphics | MaskType,
            
        //Undefined
        CategoryUndefined =  (uint)ResourceCategoryEnum.Undefined << 16,
        UndefinedOther = CategoryUndefined | MaskType,
        
            
            
        //Mask
        MaskType = 0x0000FFFF
    }
    
    
    public enum ResourceCategoryEnum : ushort
    {
        Text = 0x0001,
        Graphics = 0x0002,
        Undefined = 0xFFFF
            
    }

    public static class ResourceTypeExtension {

        public static bool IsCategory(this ResourceTypeEnum type)
        {
            return ((uint)type & 0x0000FFFF) == 0;
        }
            
        public static bool IsOther(this ResourceTypeEnum type)
        {
            return ((uint)type & 0x0000FFFF) == 0x0000FFFF;
        }
            
        public static bool IsValid(this ResourceTypeEnum type)
        {
            return !type.IsCategory() && ((uint)type & 0xFFFF0000) != 0;
        }
        
        
        public static ResourceCategoryEnum Category(this ResourceTypeEnum type)
        {
            return (ResourceCategoryEnum) ((uint) type >> 16);
        }
        
        
        
        public static ResourceTypeEnum ToOther(this ResourceCategoryEnum category)
        {
            return ((uint) category << 16)+ResourceTypeEnum.MaskType;
        }
    }
}