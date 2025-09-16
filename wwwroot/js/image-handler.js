// Image handler for missing images
function handleImageError(img) {
    if (img.dataset.fallback) return; // Prevent infinite loop
    img.dataset.fallback = 'true';
    
    // Try different fallback based on image type
    if (img.src.includes('nike')) {
        img.src = 'https://via.placeholder.com/400x300/FF6B6B/FFFFFF?text=Nike+Air+Max';
    } else if (img.src.includes('adidas')) {
        img.src = 'https://via.placeholder.com/400x300/4ECDC4/FFFFFF?text=Adidas+Ultraboost';
    } else if (img.src.includes('converse')) {
        img.src = 'https://via.placeholder.com/400x300/45B7D1/FFFFFF?text=Converse';
    } else if (img.src.includes('vans')) {
        img.src = 'https://via.placeholder.com/400x300/58D68D/FFFFFF?text=Vans';
    } else if (img.src.includes('bitis')) {
        img.src = 'https://via.placeholder.com/400x300/EC7063/FFFFFF?text=Bitis';
    } else if (img.src.includes('cao-got')) {
        img.src = 'https://via.placeholder.com/400x300/F7DC6F/000000?text=Giay+Cao+Got';
    } else if (img.src.includes('oxford')) {
        img.src = 'https://via.placeholder.com/400x300/BB8FCE/FFFFFF?text=Giay+Tay';
    } else if (img.src.includes('sandal')) {
        img.src = 'https://via.placeholder.com/400x300/F8C471/000000?text=Sandal';
    } else if (img.src.includes('boot')) {
        img.src = 'https://via.placeholder.com/400x300/85929E/FFFFFF?text=Boot';
    } else if (img.src.includes('banner')) {
        img.src = 'https://via.placeholder.com/800x400/667eea/FFFFFF?text=Banner';
    } else {
        img.src = 'https://via.placeholder.com/400x300/e9ecef/6c757d?text=Product+Image';
    }
    
    img.alt = 'Hình ảnh sản phẩm';
}

// Add error handling to all images
document.addEventListener('DOMContentLoaded', function() {
    // Handle all images on the page
    const allImages = document.querySelectorAll('img');
    allImages.forEach(img => {
        img.addEventListener('error', function() {
            handleImageError(this);
        });
        
        // Also handle images that might already be broken
        if (!img.complete || img.naturalHeight === 0) {
            handleImageError(img);
        }
    });
});