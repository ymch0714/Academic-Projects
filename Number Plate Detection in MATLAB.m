%% Assingment 2 - ENEL808 - Computer Vision 2020 - Min Choi 1132188
vid = VideoReader('vid1.mp4'); %Change to the file name of video that you want to process
vidOut = VideoWriter('NumberPlateVid.avi'); %Name of the output video file
vidOut.FrameRate = 8; %Output video framerate
blobAnalysis = vision.BlobAnalysis('BoundingBoxOutputPort', true, ...
    'AreaOutputPort', false, 'CentroidOutputPort', false, ...
    'MinimumBlobArea', 30000); %Need to change depending on the size of numberplate
open(vidOut);
while hasFrame(vid)
    I  = readFrame(vid);
    g = rgb2gray(I);
    g = imsharpen(g);
    th = graythresh(g);
    bw = imbinarize(g);
    CC = bwconncomp(bw);
    stats = regionprops(CC);
    threshold = 100000; %Need to change depending on the size of numberplate
    removeBig = [stats.Area]>threshold;
    bw(cat(1,CC.PixelIdxList{removeBig})) = false;
    se = strel('square',5);
    bw = imopen(bw,se);
    CC2 = bwconncomp(bw);
    stats2 = regionprops(CC2);
    removeSmall = [stats2.Area]<200; %Need to change depending on the size of numberplate
    bw(cat(1,CC2.PixelIdxList{removeSmall})) = false;

    bbox = blobAnalysis(bw);

    [numRows,numCols] = size(bbox);
    if numRows==1&&numCols==4
%         roi = [bbox(1), bbox(2), bbox(3), bbox(4)];
        x = [bbox(1) (bbox(1)+bbox(3)) (bbox(1)+bbox(3)) bbox(1)];
        y = [bbox(2) bbox(2) (bbox(2)+bbox(4)) (bbox(2)+bbox(4))];
        mask = roipoly(bw,x,y);
        bw = bw-(~mask);
        se2 = strel('line',10, 80);
        bw = imclose(bw, se2);
        se3 = strel('line',5,0);
        bw = imerode(bw, se3);
        ocrResults = ocr(bw,'TextLayout','Block');
        confidence = ocrResults.WordConfidences > 0.8;
        recognizedText = ocrResults.Words(confidence);
        if size(ocrResults.Text) > 0
            Iocr = insertObjectAnnotation(I, 'rectangle', ...
                       ocrResults.WordBoundingBoxes, ...
                       ocrResults.Words);
            final = insertShape(Iocr, 'Rectangle', bbox, 'Color', 'green',...
                                          'Linewidth', 3);
            xi = double(bbox(1));
            yi = double(bbox(2))-50;
            imshow(final);
            hold on
            text(xi, yi, recognizedText, 'BackgroundColor', [1 1 1], 'FontSize', 22);
            hold off
            F = getframe(gcf);
            pause(0.001)        
        else
        imshow(I);
        F = getframe(gcf);
        pause(0.001)
        end

    else
        imshow(I);
        F = getframe(gcf);
        pause(0.001)
    end
    for ii=1:length(F)
        frame = F(ii);
        writeVideo(vidOut, frame);
    end
end
close(vidOut);