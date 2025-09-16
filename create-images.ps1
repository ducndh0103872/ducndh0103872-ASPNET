# PowerShell script to create placeholder images
$products = @(
    @{name="vans-old-skool"; color="58D68D"; text="Vans+Old+Skool"},
    @{name="cao-got-mui-nhon"; color="F7DC6F"; text="Giay+Cao+Got"},
    @{name="giay-tay-oxford"; color="BB8FCE"; text="Giay+Tay+Oxford"},
    @{name="sandal-de-xuong"; color="F8C471"; text="Sandal+De+Xuong"},
    @{name="boot-da-nam"; color="85929E"; text="Boot+Da+Nam"},
    @{name="bitis-hunter-street"; color="EC7063"; text="Bitis+Hunter"},
    @{name="cao-got-de-bet"; color="A569BD"; text="Cao+Got+De+Bet"},
    @{name="default"; color="e9ecef"; text="Product+Image"}
)

$banners = @(
    @{name="banner1"; color="667eea"; text="Bo+Suu+Tap+Giay+The+Thao+2025"; size="800x400"},
    @{name="banner2"; color="f093fb"; text="Giay+Cao+Got+Thanh+Lich"; size="800x400"},
    @{name="banner3"; color="4facfe"; text="Sale+Up+To+50%25"; size="800x400"}
)

# Create product images
foreach ($product in $products) {
    $url = "https://dummyimage.com/400x300/" + $product.color + "/ffffff&text=" + $product.text
    $output = "wwwroot\images\products\" + $product.name + ".jpg"
    try {
        Invoke-WebRequest -Uri $url -OutFile $output
        Write-Host "Created: $output"
    } catch {
        Write-Host "Failed to create: $output"
    }
}

# Create banner images
foreach ($banner in $banners) {
    $url = "https://dummyimage.com/" + $banner.size + "/" + $banner.color + "/ffffff&text=" + $banner.text
    $output = "wwwroot\images\banners\" + $banner.name + ".jpg"
    try {
        Invoke-WebRequest -Uri $url -OutFile $output
        Write-Host "Created: $output"
    } catch {
        Write-Host "Failed to create: $output"
    }
}

# Create category default
$url = "https://dummyimage.com/120x120/e9ecef/6c757d&text=Category"
$output = "wwwroot\images\categories\default.jpg"
try {
    Invoke-WebRequest -Uri $url -OutFile $output
    Write-Host "Created: $output"
} catch {
    Write-Host "Failed to create: $output"
}